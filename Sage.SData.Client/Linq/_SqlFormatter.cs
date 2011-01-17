// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace IQToolkit.Data.Common
{
    /// <summary>
    /// Formats a query expression into common SQL language syntax
    /// </summary>
    public class _SqlFormatter : DbExpressionVisitor
    {
        private readonly StringBuilder _sb;
        private readonly Dictionary<TableAlias, string> _aliases;
        private bool _isNested;

        private _SqlFormatter()
        {
            _sb = new StringBuilder();
            _aliases = new Dictionary<TableAlias, string>();
        }

        public static string Format(Expression expression)
        {
            var formatter = new _SqlFormatter();
            formatter.Visit(expression);
            return formatter.ToString();
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        private bool HideColumnAliases { get; set; }

        private bool HideTableAliases { get; set; }

        private void Write(object value)
        {
            _sb.Append(value);
        }

        private void WriteParameterName(string name)
        {
            Write("@" + name);
        }

        private void WriteVariableName(string name)
        {
            WriteParameterName(name);
        }

        private void WriteAsAliasName(string aliasName)
        {
            Write("AS ");
            WriteAliasName(aliasName);
        }

        private void WriteAliasName(string aliasName)
        {
            Write(aliasName);
        }

        private void WriteAsColumnName(string columnName)
        {
            Write("AS ");
            Write(columnName);
        }

        private string GetAliasName(TableAlias alias)
        {
            string name;
            if (!_aliases.TryGetValue(alias, out name))
            {
                name = "A" + alias.GetHashCode() + "?";
                _aliases.Add(alias, name);
            }
            return name;
        }

        private void AddAlias(TableAlias alias)
        {
            string name;
            if (!_aliases.TryGetValue(alias, out name))
            {
                name = "t" + _aliases.Count;
                _aliases.Add(alias, name);
            }
        }

        private void AddAliases(Expression expr)
        {
            var ax = expr as AliasedExpression;
            if (ax != null)
            {
                AddAlias(ax.Alias);
            }
            else
            {
                var jx = expr as JoinExpression;
                if (jx != null)
                {
                    AddAliases(jx.Left);
                    AddAliases(jx.Right);
                }
            }
        }

        protected override Expression Visit(Expression exp)
        {
            if (exp == null) return null;

            // check for supported node types first 
            // non-supported ones should not be visited (as they would produce bad SQL)
            switch (exp.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.UnaryPlus:
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.Power:
                case ExpressionType.Conditional:
                case ExpressionType.Constant:
                case ExpressionType.MemberAccess:
                case ExpressionType.Call:
                case ExpressionType.New:
                case (ExpressionType) DbExpressionType.Table:
                case (ExpressionType) DbExpressionType.Column:
                case (ExpressionType) DbExpressionType.Select:
                case (ExpressionType) DbExpressionType.Join:
                case (ExpressionType) DbExpressionType.Aggregate:
                case (ExpressionType) DbExpressionType.Scalar:
                case (ExpressionType) DbExpressionType.Exists:
                case (ExpressionType) DbExpressionType.In:
                case (ExpressionType) DbExpressionType.AggregateSubquery:
                case (ExpressionType) DbExpressionType.IsNull:
                case (ExpressionType) DbExpressionType.Between:
                case (ExpressionType) DbExpressionType.RowCount:
                case (ExpressionType) DbExpressionType.Projection:
                case (ExpressionType) DbExpressionType.NamedValue:
                case (ExpressionType) DbExpressionType.Insert:
                case (ExpressionType) DbExpressionType.Update:
                case (ExpressionType) DbExpressionType.Delete:
                case (ExpressionType) DbExpressionType.Block:
                case (ExpressionType) DbExpressionType.If:
                case (ExpressionType) DbExpressionType.Declaration:
                case (ExpressionType) DbExpressionType.Variable:
                case (ExpressionType) DbExpressionType.Function:
                    return base.Visit(exp);

                default:
                    throw new NotSupportedException(string.Format("The LINQ expression node of type {0} is not supported", exp.NodeType));
            }
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            throw new NotSupportedException(string.Format("The member access '{0}' is not supported", m.Member));
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof (Decimal))
            {
                switch (m.Method.Name)
                {
                    case "Add":
                    case "Subtract":
                    case "Multiply":
                    case "Divide":
                    case "Remainder":
                        Write("(");
                        VisitValue(m.Arguments[0]);
                        Write(" ");
                        Write(GetOperator(m.Method.Name));
                        Write(" ");
                        VisitValue(m.Arguments[1]);
                        Write(")");
                        return m;
                    case "Negate":
                        Write("-");
                        Visit(m.Arguments[0]);
                        Write("");
                        return m;
                    case "Compare":
                        Visit(Expression.Condition(
                            Expression.Equal(m.Arguments[0], m.Arguments[1]),
                            Expression.Constant(0),
                            Expression.Condition(
                                Expression.LessThan(m.Arguments[0], m.Arguments[1]),
                                Expression.Constant(-1),
                                Expression.Constant(1)
                                )));
                        return m;
                }
            }
            else if (m.Method.Name == "ToString" && m.Object.Type == typeof (string))
            {
                return Visit(m.Object); // no op
            }
            else if (m.Method.Name == "Equals")
            {
                if (m.Method.IsStatic && m.Method.DeclaringType == typeof (object))
                {
                    Write("(");
                    Visit(m.Arguments[0]);
                    Write(" = ");
                    Visit(m.Arguments[1]);
                    Write(")");
                    return m;
                }
                if (!m.Method.IsStatic && m.Arguments.Count == 1 && m.Arguments[0].Type == m.Object.Type)
                {
                    Write("(");
                    Visit(m.Object);
                    Write(" = ");
                    Visit(m.Arguments[0]);
                    Write(")");
                    return m;
                }
            }
            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        protected override NewExpression VisitNew(NewExpression nex)
        {
            throw new NotSupportedException(string.Format("The construtor for '{0}' is not supported", nex.Constructor.DeclaringType));
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            var op = GetOperator(u);
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    if (IsBoolean(u.Operand.Type) || op.Length > 1)
                    {
                        Write(op);
                        Write(" ");
                        VisitPredicate(u.Operand);
                    }
                    else
                    {
                        Write(op);
                        VisitValue(u.Operand);
                    }
                    break;
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    Write(op);
                    VisitValue(u.Operand);
                    break;
                case ExpressionType.UnaryPlus:
                    VisitValue(u.Operand);
                    break;
                case ExpressionType.Convert:
                    // ignore conversions for now
                    Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            var op = GetOperator(b);
            var left = b.Left;
            var right = b.Right;

            Write("(");
            switch (b.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    if (IsBoolean(left.Type))
                    {
                        VisitPredicate(left);
                        Write(" ");
                        Write(op);
                        Write(" ");
                        VisitPredicate(right);
                    }
                    else
                    {
                        VisitValue(left);
                        Write(" ");
                        Write(op);
                        Write(" ");
                        VisitValue(right);
                    }
                    break;
                case ExpressionType.Equal:
                    if (right.NodeType == ExpressionType.Constant)
                    {
                        var ce = (ConstantExpression) right;
                        if (ce.Value == null)
                        {
                            Visit(left);
                            Write(" IS NULL");
                            break;
                        }
                    }
                    else if (left.NodeType == ExpressionType.Constant)
                    {
                        var ce = (ConstantExpression) left;
                        if (ce.Value == null)
                        {
                            Visit(right);
                            Write(" IS NULL");
                            break;
                        }
                    }
                    goto case ExpressionType.LessThan;
                case ExpressionType.NotEqual:
                    if (right.NodeType == ExpressionType.Constant)
                    {
                        var ce = (ConstantExpression) right;
                        if (ce.Value == null)
                        {
                            Visit(left);
                            Write(" IS NOT NULL");
                            break;
                        }
                    }
                    else if (left.NodeType == ExpressionType.Constant)
                    {
                        var ce = (ConstantExpression) left;
                        if (ce.Value == null)
                        {
                            Visit(right);
                            Write(" IS NOT NULL");
                            break;
                        }
                    }
                    goto case ExpressionType.LessThan;
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                    // check for special x.CompareTo(y) && type.Compare(x,y)
                    if (left.NodeType == ExpressionType.Call && right.NodeType == ExpressionType.Constant)
                    {
                        var mc = (MethodCallExpression) left;
                        var ce = (ConstantExpression) right;
                        if (ce.Value != null && ce.Value.GetType() == typeof (int) && ((int) ce.Value) == 0)
                        {
                            if (mc.Method.Name == "CompareTo" && !mc.Method.IsStatic && mc.Arguments.Count == 1)
                            {
                                left = mc.Object;
                                right = mc.Arguments[0];
                            }
                            else if (
                                (mc.Method.DeclaringType == typeof (string) || mc.Method.DeclaringType == typeof (decimal))
                                && mc.Method.Name == "Compare" && mc.Method.IsStatic && mc.Arguments.Count == 2)
                            {
                                left = mc.Arguments[0];
                                right = mc.Arguments[1];
                            }
                        }
                    }
                    goto case ExpressionType.Add;
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.LeftShift:
                case ExpressionType.RightShift:
                    VisitValue(left);
                    Write(" ");
                    Write(op);
                    Write(" ");
                    VisitValue(right);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }
            Write(")");
            return b;
        }

        private static string GetOperator(string methodName)
        {
            switch (methodName)
            {
                case "Add":
                    return "+";
                case "Subtract":
                    return "-";
                case "Multiply":
                    return "*";
                case "Divide":
                    return "/";
                case "Negate":
                    return "-";
                case "Remainder":
                    return "%";
                default:
                    return null;
            }
        }

        private static string GetOperator(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    return "-";
                case ExpressionType.UnaryPlus:
                    return "+";
                case ExpressionType.Not:
                    return IsBoolean(u.Operand.Type) ? "NOT" : "~";
                default:
                    return "";
            }
        }

        private static string GetOperator(BinaryExpression b)
        {
            switch (b.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return (IsBoolean(b.Left.Type)) ? "AND" : "&";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return (IsBoolean(b.Left.Type) ? "OR" : "|");
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Modulo:
                    return "%";
                case ExpressionType.ExclusiveOr:
                    return "^";
                case ExpressionType.LeftShift:
                    return "<<";
                case ExpressionType.RightShift:
                    return ">>";
                default:
                    return "";
            }
        }

        private static bool IsBoolean(Type type)
        {
            return type == typeof (bool) || type == typeof (bool?);
        }

        private static bool IsPredicate(Expression expr)
        {
            switch (expr.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return IsBoolean(expr.Type);
                case ExpressionType.Not:
                    return IsBoolean(expr.Type);
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case (ExpressionType) DbExpressionType.IsNull:
                case (ExpressionType) DbExpressionType.Between:
                case (ExpressionType) DbExpressionType.Exists:
                case (ExpressionType) DbExpressionType.In:
                    return true;
                case ExpressionType.Call:
                    return IsBoolean(expr.Type);
                default:
                    return false;
            }
        }

        private void VisitPredicate(Expression expr)
        {
            Visit(expr);
            if (!IsPredicate(expr))
            {
                Write(" <> 0");
            }
        }

        private Expression VisitValue(Expression expr)
        {
            return Visit(expr);
        }

        protected override Expression VisitConditional(ConditionalExpression c)
        {
            throw new NotSupportedException(string.Format("Conditional expressions not supported"));
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            WriteValue(c.Value);
            return c;
        }

        private void WriteValue(object value)
        {
            if (value == null)
            {
                Write("NULL");
            }
            else if (value.GetType().IsEnum)
            {
                Write(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())));
            }
            else
            {
                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.Boolean:
                        Write(((bool) value) ? 1 : 0);
                        break;

                    case TypeCode.String:
                        Write("'");
                        Write(value);
                        Write("'");
                        break;

                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", value));

                    case TypeCode.Single:
                    case TypeCode.Double:
                        var str = value.ToString();
                        if (!str.Contains('.'))
                        {
                            str += ".0";
                        }
                        Write(str);
                        break;

                    default:
                        Write(value);
                        break;
                }
            }
        }

        protected override Expression VisitColumn(ColumnExpression column)
        {
            if (column.Alias != null && !HideColumnAliases)
            {
                WriteAliasName(GetAliasName(column.Alias));
                Write(".");
            }
            Write(column.Name);
            return column;
        }

        protected override Expression VisitProjection(ProjectionExpression proj)
        {
            // treat these like scalar subqueries
            if (proj.Projector is ColumnExpression)
            {
                Write("(");
                Visit(proj.Select);
                Write(")");
            }
            else
            {
                throw new NotSupportedException("Non-scalar projections cannot be translated to SQL.");
            }
            return proj;
        }

        protected override Expression VisitSelect(SelectExpression select)
        {
            AddAliases(select.From);
            Write("SELECT ");

            if (select.IsDistinct)
            {
                Write("DISTINCT ");
            }

            if (select.Take != null)
            {
                WriteTopClause(select.Take);
            }

            WriteColumns(select.Columns);

            if (select.From != null)
            {
                Write("FROM ");
                VisitSource(select.From);
            }

            if (select.Where != null)
            {
                Write("WHERE ");
                VisitPredicate(select.Where);
            }

            if (select.GroupBy != null && select.GroupBy.Count > 0)
            {
                Write("GROUP BY ");
                for (var i = 0; i < select.GroupBy.Count; i++)
                {
                    if (i > 0)
                    {
                        Write(", ");
                    }
                    VisitValue(select.GroupBy[i]);
                }
            }

            if (select.OrderBy != null && select.OrderBy.Count > 0)
            {
                Write("ORDER BY ");
                for (var i = 0; i < select.OrderBy.Count; i++)
                {
                    var exp = select.OrderBy[i];
                    if (i > 0)
                    {
                        Write(", ");
                    }
                    VisitValue(exp.Expression);
                    if (exp.OrderType != OrderType.Ascending)
                    {
                        Write(" DESC");
                    }
                }
            }

            return select;
        }

        private void WriteTopClause(Expression expression)
        {
            Write("TOP (");
            Visit(expression);
            Write(") ");
        }

        private void WriteColumns(ReadOnlyCollection<ColumnDeclaration> columns)
        {
            if (columns.Count > 0)
            {
                for (var i = 0; i < columns.Count; i++)
                {
                    var column = columns[i];
                    if (i > 0)
                    {
                        Write(", ");
                    }
                    var c = VisitValue(column.Expression) as ColumnExpression;
                    if (!string.IsNullOrEmpty(column.Name) && (c == null || c.Name != column.Name))
                    {
                        Write(" ");
                        WriteAsColumnName(column.Name);
                    }
                }
            }
            else
            {
                Write("NULL ");
                if (_isNested)
                {
                    WriteAsColumnName("tmp");
                    Write(" ");
                }
            }
        }

        protected override Expression VisitSource(Expression source)
        {
            var saveIsNested = _isNested;
            _isNested = true;
            switch ((DbExpressionType) source.NodeType)
            {
                case DbExpressionType.Table:
                    var table = (TableExpression) source;
                    Write(table.Name);
                    if (!HideTableAliases)
                    {
                        Write(" ");
                        WriteAsAliasName(GetAliasName(table.Alias));
                    }
                    break;
                case DbExpressionType.Select:
                    var select = (SelectExpression) source;
                    Write("(");
                    Visit(select);
                    Write(") ");
                    WriteAsAliasName(GetAliasName(select.Alias));
                    break;
                case DbExpressionType.Join:
                    VisitJoin((JoinExpression) source);
                    break;
                default:
                    throw new InvalidOperationException("Select source is not valid type");
            }
            _isNested = saveIsNested;
            return source;
        }

        protected override Expression VisitJoin(JoinExpression join)
        {
            VisitJoinLeft(join.Left);
            switch (join.Join)
            {
                case JoinType.CrossJoin:
                    Write("CROSS JOIN ");
                    break;
                case JoinType.InnerJoin:
                    Write("INNER JOIN ");
                    break;
                case JoinType.CrossApply:
                    Write("CROSS APPLY ");
                    break;
                case JoinType.OuterApply:
                    Write("OUTER APPLY ");
                    break;
                case JoinType.LeftOuter:
                case JoinType.SingletonLeftOuter:
                    Write("LEFT OUTER JOIN ");
                    break;
            }
            VisitJoinRight(join.Right);
            if (join.Condition != null)
            {
                Write("ON ");
                VisitPredicate(join.Condition);
            }
            return join;
        }

        private void VisitJoinLeft(Expression source)
        {
            VisitSource(source);
        }

        private void VisitJoinRight(Expression source)
        {
            VisitSource(source);
        }

        private void WriteAggregateName(string aggregateName)
        {
            switch (aggregateName)
            {
                case "Average":
                    Write("AVG");
                    break;
                case "LongCount":
                    Write("COUNT");
                    break;
                default:
                    Write(aggregateName.ToUpper());
                    break;
            }
        }

        private static bool RequiresAsteriskWhenNoArgument(string aggregateName)
        {
            return aggregateName == "Count" || aggregateName == "LongCount";
        }

        protected override Expression VisitAggregate(AggregateExpression aggregate)
        {
            WriteAggregateName(aggregate.AggregateName);
            Write("(");
            if (aggregate.IsDistinct)
            {
                Write("DISTINCT ");
            }
            if (aggregate.Argument != null)
            {
                VisitValue(aggregate.Argument);
            }
            else if (RequiresAsteriskWhenNoArgument(aggregate.AggregateName))
            {
                Write("*");
            }
            Write(")");
            return aggregate;
        }

        protected override Expression VisitIsNull(IsNullExpression isnull)
        {
            VisitValue(isnull.Expression);
            Write(" IS NULL");
            return isnull;
        }

        protected override Expression VisitBetween(BetweenExpression between)
        {
            VisitValue(between.Expression);
            Write(" BETWEEN ");
            VisitValue(between.Lower);
            Write(" AND ");
            VisitValue(between.Upper);
            return between;
        }

        protected override Expression VisitRowNumber(RowNumberExpression rowNumber)
        {
            throw new NotSupportedException();
        }

        protected override Expression VisitScalar(ScalarExpression subquery)
        {
            Write("(");
            Visit(subquery.Select);
            Write(")");
            return subquery;
        }

        protected override Expression VisitExists(ExistsExpression exists)
        {
            Write("EXISTS(");
            Visit(exists.Select);
            Write(")");
            return exists;
        }

        protected override Expression VisitIn(InExpression @in)
        {
            if (@in.Values != null)
            {
                if (@in.Values.Count == 0)
                {
                    Write("0 <> 0");
                }
                else
                {
                    VisitValue(@in.Expression);
                    Write(" IN (");
                    for (var i = 0; i < @in.Values.Count; i++)
                    {
                        if (i > 0) Write(", ");
                        VisitValue(@in.Values[i]);
                    }
                    Write(")");
                }
            }
            else
            {
                VisitValue(@in.Expression);
                Write(" IN (");
                Visit(@in.Select);
                Write(")");
            }
            return @in;
        }

        protected override Expression VisitNamedValue(NamedValueExpression value)
        {
            WriteParameterName(value.Name);
            return value;
        }

        protected override Expression VisitInsert(InsertCommand insert)
        {
            Write("INSERT INTO ");
            Write(insert.Table.Name);
            Write("(");
            for (var i = 0; i < insert.Assignments.Count; i++)
            {
                var ca = insert.Assignments[i];
                if (i > 0) Write(", ");
                Write(ca.Column.Name);
            }
            Write(")");
            Write("VALUES (");
            for (var i = 0; i < insert.Assignments.Count; i++)
            {
                var ca = insert.Assignments[i];
                if (i > 0) Write(", ");
                Visit(ca.Expression);
            }
            Write(")");
            return insert;
        }

        protected override Expression VisitUpdate(UpdateCommand update)
        {
            Write("UPDATE ");
            Write(update.Table.Name);
            var saveHide = HideColumnAliases;
            HideColumnAliases = true;
            Write("SET ");
            for (var i = 0; i < update.Assignments.Count; i++)
            {
                var ca = update.Assignments[i];
                if (i > 0) Write(", ");
                Visit(ca.Column);
                Write(" = ");
                Visit(ca.Expression);
            }
            if (update.Where != null)
            {
                Write("WHERE ");
                VisitPredicate(update.Where);
            }
            HideColumnAliases = saveHide;
            return update;
        }

        protected override Expression VisitDelete(DeleteCommand delete)
        {
            Write("DELETE FROM ");
            var saveHideTable = HideTableAliases;
            var saveHideColumn = HideColumnAliases;
            HideTableAliases = true;
            HideColumnAliases = true;
            VisitSource(delete.Table);
            if (delete.Where != null)
            {
                Write("WHERE ");
                VisitPredicate(delete.Where);
            }
            HideTableAliases = saveHideTable;
            HideColumnAliases = saveHideColumn;
            return delete;
        }

        protected override Expression VisitIf(IFCommand ifx)
        {
            throw new NotSupportedException();
        }

        protected override Expression VisitBlock(BlockCommand block)
        {
            throw new NotSupportedException();
        }

        protected override Expression VisitDeclaration(DeclarationCommand decl)
        {
            throw new NotSupportedException();
        }

        protected override Expression VisitVariable(VariableExpression vex)
        {
            WriteVariableName(vex.Name);
            return vex;
        }

        protected override Expression VisitFunction(FunctionExpression func)
        {
            Write(func.Name);
            if (func.Arguments.Count > 0)
            {
                Write("(");
                for (var i = 0; i < func.Arguments.Count; i++)
                {
                    if (i > 0) Write(", ");
                    Visit(func.Arguments[i]);
                }
                Write(")");
            }
            return func;
        }
    }
}