using OpenSource.DB.Repository.Attributes;
using OpenSource.DB.Repository.Attributes.Joins;
using OpenSource.DB.Repository.Extensions;
using OpenSource.Helps.DB.DbAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Policy;
using System.Text;

namespace OpenSource.DB.Repository.SqlGenerator
{
    public class SqlGenerator<TEntity> : ISqlGenerator<TEntity>
        where TEntity : class
    {
        public SqlGenerator(ESqlConnector sqlConnector)
        {
            SqlConnector = sqlConnector;
            var entityType = typeof(TEntity);
            var entityTypeInfo = entityType.GetTypeInfo();
            var aliasAttribute = entityTypeInfo.GetCustomAttribute<TableAttribute>();

            this.TableName = aliasAttribute != null ? aliasAttribute.Name : entityTypeInfo.Name;
            AllProperties = entityType.GetProperties();
            //Load all the "primitive" entity properties
            var props = AllProperties.Where(ExpressionHelper.GetPrimitivePropertiesPredicate()).ToArray();

            //Filter the non stored properties
            this.BaseProperties =
                props.Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any())
                    .Select(p => new PropertyMetadata(p));

            //Filter key properties
            this.KeyProperties =
                props.Where(p => p.GetCustomAttributes<KeyAttribute>().Any()).Select(p => new PropertyMetadata(p));

            //Use identity as key pattern
            var identityProperty = props.FirstOrDefault(p => p.GetCustomAttributes<IdentityAttribute>().Any());
            this.IdentityProperty = identityProperty != null ? new PropertyMetadata(identityProperty) : null;

            //Status property (if exists, and if it does, it must be an enumeration)
            var statusProperty = props.FirstOrDefault(p => p.GetCustomAttributes<StatusAttribute>().Any());

            if (statusProperty == null) return;
            StatusProperty = new PropertyMetadata(statusProperty);

            if (statusProperty.PropertyType.IsBool())
            {
                var deleteProperty = props.FirstOrDefault(p => p.GetCustomAttributes<DeletedAttribute>().Any());
                if (deleteProperty == null) return;

                LogicalDelete = true;
                LogicalDeleteValue = 1; // true

            }
            else if (statusProperty.PropertyType.IsEnum())
            {

                var deleteOption =
                    statusProperty.PropertyType.GetFields()
                        .FirstOrDefault(f => f.GetCustomAttribute<DeletedAttribute>() != null);

                if (deleteOption == null) return;

                var enumValue = Enum.Parse(statusProperty.PropertyType, deleteOption.Name);

                if (enumValue != null)
                    LogicalDeleteValue = Convert.ChangeType(enumValue,
                        Enum.GetUnderlyingType(statusProperty.PropertyType));

                LogicalDelete = true;
            }
        }

        public SqlGenerator()
            : this(ESqlConnector.MSSQL)
        {
        }

        public ESqlConnector SqlConnector { get; set; }

        public bool IsIdentity => this.IdentityProperty != null;

        public bool LogicalDelete { get; }

        public string TableName { get; }

        public PropertyInfo[] AllProperties { get; }

        public PropertyMetadata IdentityProperty { get; }

        public IEnumerable<PropertyMetadata> KeyProperties { get; }

        public IEnumerable<PropertyMetadata> BaseProperties { get; }

        public PropertyMetadata StatusProperty { get; }

        public object LogicalDeleteValue { get; }

        public virtual SqlQuery GetInsert(TEntity entity)
        {
            List<PropertyMetadata> properties = (this.IsIdentity
                ? this.BaseProperties.Where(
                    p => !p.Name.Equals(this.IdentityProperty.Name, StringComparison.OrdinalIgnoreCase))
                : this.BaseProperties).ToList();

            string columNames = string.Join(", ", properties.Select(p => $"{p.ColumnName}"))
            ;
            string values = string.Join(", ", properties.Select(p => $"@{p.Name}"))
            ;

            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("INSERT INTO {0} {1} {2} ",
                this.TableName,
                string.IsNullOrEmpty(columNames) ? "" : $"({columNames})", string.IsNullOrEmpty(values) ? "" : $" VALUES ({values})")
            ;

            if (this.IsIdentity)
            {
                switch (SqlConnector)
                {
                    case ESqlConnector.MSSQL:
                        sqlBuilder.Append("SELECT SCOPE_IDENTITY() AS " + this.IdentityProperty.ColumnName);
                        break;

                    case ESqlConnector.MySQL:
                        sqlBuilder.Append("; SELECT CONVERT(LAST_INSERT_ID(), SIGNED INTEGER) AS " +
                                          this.IdentityProperty.ColumnName);
                        break;

                    case ESqlConnector.PostgreSQL:
                        sqlBuilder.Append("RETURNING " + this.IdentityProperty.ColumnName);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return new SqlQuery(sqlBuilder.ToString(), entity);
        }

        public virtual SqlQuery GetUpdate(TEntity entity)
        {
            var properties =
                this.BaseProperties.Where(
                    p => !this.KeyProperties.Any(k => k.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase)));

            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE {0} SET {1} WHERE {2}", this.TableName,
                string.Join(", ", properties.Select(p => $"{p.ColumnName} = @{p.Name}")), string.Join(" AND ", this.KeyProperties.Select(p => $"{p.ColumnName} = @{p.Name}")))
            ;

            return new SqlQuery(sqlBuilder.ToString().TrimEnd(), entity);
        }

        #region Get Select

        public virtual SqlQuery GetSelectFirst(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return GetSelect(predicate, true, includes);
        }

        public virtual SqlQuery GetSelectAll(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return GetSelect(predicate, false, includes);
        }


        private StringBuilder InitBuilderSelect(bool firstOnly)
        {
            var builder = new StringBuilder();
            var select = "SELECT ";

            if (firstOnly && SqlConnector == ESqlConnector.MSSQL)
                select += "TOP 1 ";

            // convert the query parms into a SQL string and dynamic property object
            builder.Append($"{select} {GetFieldsSelect(TableName, BaseProperties)}");

            return builder;
        }


        private StringBuilder AppendListToSelect(StringBuilder originalBuilder, ref List<QueryParameter> queryProperties,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var joinsBuilder = new StringBuilder();

            foreach (var include in includes)
            {
                var propertyName = ExpressionHelper.GetPropertyName(include);
                var joinProperty = AllProperties.First(x => x.Name == propertyName);
                var attrJoin = joinProperty.GetCustomAttribute<JoinAttributeBase>();
                if (attrJoin != null)
                {
                    var joinString = "";
                    if (attrJoin is LeftJoinAttribute)
                    {
                        joinString = "LEFT JOIN ";
                    }
                    else if (attrJoin is InnerJoinAttribute)
                    {
                        joinString = "INNER JOIN ";
                    }
                    else if (attrJoin is RightJoinAttribute)
                    {
                        joinString = "RIGHT JOIN ";
                    }

                    var joinType = joinProperty.PropertyType.IsGenericType()
                        ? joinProperty.PropertyType.GenericTypeArguments[0]
                        : joinProperty.PropertyType;

                    var properties = joinType.GetProperties().Where(ExpressionHelper.GetPrimitivePropertiesPredicate());
                    var props =
                        properties.Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any())
                            .Select(p => new PropertyMetadata(p));
                    originalBuilder.Append(", " + GetFieldsSelect(attrJoin.TableName, props));


                    joinsBuilder.Append($"{joinString} {attrJoin.TableName} ON {TableName}.{attrJoin.Key} = {attrJoin.TableName}.{attrJoin.ExternalKey} ")
                    ;
                }
            }
            return joinsBuilder;
        }

        private static string GetFieldsSelect(string tableName, IEnumerable<PropertyMetadata> properties)
        {
            //Projection function
            Func<PropertyMetadata, string> projectionFunction = (p) => !string.IsNullOrEmpty(p.Alias) ? $"{tableName}.{p.ColumnName} AS {p.Name}" : $"{tableName}.{p.ColumnName}";

            return string.Join(", ", properties.Select(projectionFunction));
        }


        private SqlQuery GetSelect(Expression<Func<TEntity, bool>> predicate, bool firstOnly,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var builder = InitBuilderSelect(firstOnly);

            builder.Append($" FROM {TableName} ");

            IDictionary<string, object> expando = new ExpandoObject();

            if (predicate != null)
            {
                // WHERE
                var queryProperties = new List<QueryParameter>();

                FillQueryProperties(ExpressionHelper.GetExpression(predicate.Body), ExpressionType.Default,
                    ref queryProperties);

                builder.Append(" WHERE ");


                for (int i = 0; i < queryProperties.Count; i++)
                {
                    var item = queryProperties[i];
                    if (i == 0)
                        item.LinkingOperator = null;
                    GetQueryParameterQueryOperator(item, ref expando, ref builder);
                }
            }

            if (firstOnly && (SqlConnector == ESqlConnector.MySQL || SqlConnector == ESqlConnector.PostgreSQL))
                builder.Append("LIMIT 1");


            return new SqlQuery(builder.ToString().TrimEnd(), expando);
        }

        private void GetQueryParameterQueryOperator(QueryParameter item, ref IDictionary<string, object> obj, ref StringBuilder builder)
        {
            switch (item.QueryOperator)
            {
                case "In":
                case "Not_In":
                    builder.Append(string.Format("{0}.{1} {2} ({3}) ", TableName, item.PropertyName,
                             item.QueryOperator.Replace("_"," "), item.PropertyValue));
                    break;
                case "Like":
                case "Not_Like":
                    builder.Append(string.Format("{0}.{1} {2} '{3}' ", TableName, item.PropertyName,
                         item.QueryOperator.Replace("_", " "), item.PropertyValue));
                    break;
                default:
                    builder.Append(string.Format("{0} {1}.{2} {3} @{2} ", item.LinkingOperator, TableName,
                              item.PropertyName, item.QueryOperator));
                    obj[item.PropertyName] = item.PropertyValue;
                    break;
            }
        }

        public virtual SqlQuery GetSelectBetween(object from, object to, Expression<Func<TEntity, object>> btwField,
            Expression<Func<TEntity, bool>> expression)
        {
            var filedName = ExpressionHelper.GetPropertyName(btwField);
            var queryResult = GetSelectAll(expression);
            var op = expression == null ? "WHERE" : "AND";

            queryResult.AppendToSql($" {op} {filedName} BETWEEN '{@from}' AND '{to}'");

            return queryResult;
        }

        public virtual SqlQuery GetDelete(TEntity entity)
        {
            var sqlBuilder = new StringBuilder();

            if (!LogicalDelete)
            {
                sqlBuilder.AppendFormat("DELETE FROM {0} WHERE {1}", this.TableName,
                    string.Join(" AND ", this.KeyProperties.Select(p => $"{p.ColumnName} = @{p.Name}")));
            }
            else
            {
                sqlBuilder.AppendFormat("UPDATE {0} SET {1} WHERE {2}", this.TableName, $"{this.StatusProperty.ColumnName} = {this.LogicalDeleteValue}", string.Join(" AND ", this.KeyProperties.Select(p => $"{p.ColumnName} = @{p.Name}")))
                ;
            }

            return new SqlQuery(sqlBuilder.ToString(), entity);
        }


        public virtual SqlQuery GetSelectCount(string sql, object param)
        {
            return new SqlQuery(string.Format("SELECT COUNT(*) FROM ({0}) Repository", sql), param);
        }

        public virtual SqlQuery GetSelectPages(long from, long to, string sql, object param)
        {
            var sqlBuilder = new StringBuilder();
            if (this.IsIdentity)
            {
                switch (SqlConnector)
                {
                    case ESqlConnector.MSSQL:
                        sqlBuilder.AppendFormat(
                            "SELECT  * FROM ( SELECT ROW_NUMBER() OVER (ORDER BY {0}) AS RowNum,{1})Repository WHERE RowNum BETWEEN {2}AND {3}",
                            this.IdentityProperty.ColumnName, sql.Substring(sql.ToUpper().IndexOf("SELECT") + 6),
                            (from - 1) * to + 1, from * to);
                        break;

                    case ESqlConnector.MySQL:
                        sqlBuilder.AppendFormat(
                            "; SELECT * FROM ({0} ORDER BY {1})Repository WHERE  {1} LIMIT {2},{3} ", sql,
                            this.IdentityProperty.ColumnName, from - 1, to);
                        break;

                    case ESqlConnector.PostgreSQL:
                        sqlBuilder.AppendFormat("SELECT * FROM ({0} ORDER BY {1})Repository LIMIT {1} OFFSET {2}", sql,
                            to, (from - 1) * to);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return new SqlQuery(sqlBuilder.ToString(), param);
        }

        #endregion Get Select

        #region Expression

        /// <summary>
        /// Fill query properties
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="linkingType">Type of the linking.</param>
        /// <param name="queryProperties">The query properties.</param>
        private static void FillQueryProperties(Expression body, ExpressionType linkingType,
            ref List<QueryParameter> queryProperties)
        {
            if (body is BinaryExpression)
            {
                BinaryExpression be = (BinaryExpression)body;
                if (body.NodeType != ExpressionType.AndAlso && body.NodeType != ExpressionType.OrElse)
                {
                    string propertyName = ExpressionHelper.GetPropertyName(be);
                    object propertyValue = ExpressionHelper.GetValue(be.Right);
                    string opr = ExpressionHelper.GetSqlOperator(be.NodeType);
                    string link = ExpressionHelper.GetSqlOperator(linkingType);

                    queryProperties.Add(new QueryParameter(link, propertyName, propertyValue, opr));
                }
                else
                {
                    FillQueryProperties(ExpressionHelper.GetExpression(be.Left), body.NodeType,
                        ref queryProperties);
                    FillQueryProperties(ExpressionHelper.GetExpression(be.Right), body.NodeType,
                        ref queryProperties);

                }
            }
            else if (body is MethodCallExpression)
            {
                MethodCallExpression mce = (MethodCallExpression)body;
                string link = ExpressionHelper.GetSqlOperator(linkingType);
                string opr = mce.Method.Name;
                string propertyName = ExpressionHelper.ExpressionRouter(mce.Arguments[0]);
                object propertyValue = ExpressionHelper.ExpressionRouter(mce.Arguments[1]);
                queryProperties.Add(new QueryParameter(link, propertyName, propertyValue, opr));
            }
        }

    }


    #endregion
}
