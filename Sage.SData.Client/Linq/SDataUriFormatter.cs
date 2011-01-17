using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using IQToolkit.Data.Common;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Linq
{
    public class SDataUriFormatter : DbExpressionVisitor
    {
        private readonly StringBuilder _sb;

        private SDataUriFormatter()
        {
            _sb = new StringBuilder();
        }

        public static string Format(Expression expression)
        {
            var formatter = new SDataUriFormatter();
            formatter.Visit(expression);
            return formatter.ToString();
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        private void Write(object value)
        {
            _sb.Append(value);
        }

        protected override Expression Visit(Expression exp)
        {
            if (exp == null) return null;

            // check for supported node types first 
            // non-supported ones should not be visited (as they would produce bad SQL)
            switch (exp.NodeType)
            {
                case ExpressionType.Negate:
                    return base.Visit(exp);
                case ExpressionType.NegateChecked:
                    return base.Visit(exp);
                case ExpressionType.Not:
                    return base.Visit(exp);
                case ExpressionType.Convert:
                    return base.Visit(exp);
                case ExpressionType.ConvertChecked:
                    return base.Visit(exp);
                case ExpressionType.UnaryPlus:
                    return base.Visit(exp);
                case ExpressionType.Add:
                    return base.Visit(exp);
                case ExpressionType.AddChecked:
                    return base.Visit(exp);
                case ExpressionType.Subtract:
                    return base.Visit(exp);
                case ExpressionType.SubtractChecked:
                    return base.Visit(exp);
                case ExpressionType.Multiply:
                    return base.Visit(exp);
                case ExpressionType.MultiplyChecked:
                    return base.Visit(exp);
                case ExpressionType.Divide:
                    return base.Visit(exp);
                case ExpressionType.Modulo:
                    return base.Visit(exp);
                case ExpressionType.And:
                    return base.Visit(exp);
                case ExpressionType.AndAlso:
                    return base.Visit(exp);
                case ExpressionType.Or:
                    return base.Visit(exp);
                case ExpressionType.OrElse:
                    return base.Visit(exp);
                case ExpressionType.LessThan:
                    return base.Visit(exp);
                case ExpressionType.LessThanOrEqual:
                    return base.Visit(exp);
                case ExpressionType.GreaterThan:
                    return base.Visit(exp);
                case ExpressionType.GreaterThanOrEqual:
                    return base.Visit(exp);
                case ExpressionType.Equal:
                    return base.Visit(exp);
                case ExpressionType.NotEqual:
                    return base.Visit(exp);
                case ExpressionType.Coalesce:
                    return base.Visit(exp);
                case ExpressionType.RightShift:
                    return base.Visit(exp);
                case ExpressionType.LeftShift:
                    return base.Visit(exp);
                case ExpressionType.ExclusiveOr:
                    return base.Visit(exp);
                case ExpressionType.Power:
                    return base.Visit(exp);
                case ExpressionType.Conditional:
                    return base.Visit(exp);
                case ExpressionType.Constant:
                    return base.Visit(exp);
                case ExpressionType.MemberAccess:
                    return base.Visit(exp);
                case ExpressionType.Call:
                    return base.Visit(exp);
                case ExpressionType.New:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.Table:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.Column:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.Select:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.Join:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.Aggregate:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.Scalar:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.Exists:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.In:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.AggregateSubquery:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.IsNull:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.Between:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.RowCount:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.Projection:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.NamedValue:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.Insert:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.Update:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.Delete:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.Block:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.If:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.Declaration:
                    return base.Visit(exp);
                case (ExpressionType) DbExpressionType.Variable:
                    return base.Visit(exp);
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
                case ExpressionType.NotEqual:
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
                    return "mul";
                case "Divide":
                    return "div";
                case "Negate":
                    return "-";
                case "Remainder":
                    return "mod";
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
                    return "not";
                default:
                    return null;
            }
        }

        private static string GetOperator(BinaryExpression b)
        {
            switch (b.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return "and";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return "or";
                case ExpressionType.Equal:
                    return "eq";
                case ExpressionType.NotEqual:
                    return "ne";
                case ExpressionType.LessThan:
                    return "lt";
                case ExpressionType.LessThanOrEqual:
                    return "le";
                case ExpressionType.GreaterThan:
                    return "gt";
                case ExpressionType.GreaterThanOrEqual:
                    return "ge";
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "mul";
                case ExpressionType.Divide:
                    return "div";
                case ExpressionType.Modulo:
                    return "mod";
                default:
                    return null;
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
                Write(" ne 0");
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
                throw new NotSupportedException("Literal null constants not supported");
            }
            if (value.GetType().IsEnum)
            {
                Write(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())));
            }
            else
            {
                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.Boolean:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", value));

                    case TypeCode.String:
                    {
                        var str = (string) value;
                        var quote = str.Contains("'") ? '"' : '\'';

                        if (quote == '"' && str.Contains("\""))
                        {
                            throw new NotSupportedException("Strings containing both single and double quotes not supported");
                        }

                        Write(quote);
                        Write(value);
                        Write(quote);
                        break;
                    }

                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", value));

                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                    {
                        Write(value);
                        if (!value.ToString().Contains('.'))
                        {
                            Write(".0");
                        }
                        break;
                    }

                    case TypeCode.DateTime:
                    {
                        Write("@");
                        Write(new W3CDateTime((DateTime) value));
                        Write("@");
                        break;
                    }

                    default:
                        Write(value);
                        break;
                }
            }
        }

        protected override Expression VisitColumn(ColumnExpression column)
        {
            Write(column.Name);
            return column;
        }

        protected override Expression VisitProjection(ProjectionExpression proj)
        {
            //TODO
            return proj;
        }

        protected override Expression VisitSelect(SelectExpression select)
        {
            if (select.IsDistinct)
            {
                throw new NotSupportedException();
            }
            if (select.GroupBy != null && select.GroupBy.Count > 0)
            {
                throw new NotSupportedException();
            }

            var uri = new SDataUri();

            if (select.Skip != null)
            {
                Visit(select.Skip);
                uri.StartIndex = long.Parse(_sb.ToString());
                _sb.Length = 0;
            }

            if (select.Take != null)
            {
                Visit(select.Take);
                uri.Count = long.Parse(_sb.ToString());
                _sb.Length = 0;
            }

            var where = select.Where;

            if (select.From != null)
            {
                VisitSource(select.From);
                var text = _sb.ToString();
                _sb.Length = 0;
                string predicate;

                if (IsSingleRequest(where))
                {
                    Visit(((BinaryExpression) where).Right);
                    predicate = _sb.ToString();
                    _sb.Length = 0;
                    where = null;
                }
                else
                {
                    predicate = null;
                }

                uri.AppendPath(new UriPathSegment(text, predicate));
            }

            if (where != null)
            {
                Visit(where);
                uri.Where = _sb.ToString();
                _sb.Length = 0;
            }

            if (select.Columns != null && select.Columns.Count > 0)
            {
                VisitColumnDeclarations(select.Columns);
                uri.Select = _sb.ToString();
                _sb.Length = 0;
            }

            if (select.OrderBy != null && select.OrderBy.Count > 0)
            {
                VisitOrderBy(select.OrderBy);
                uri.OrderBy = _sb.ToString();
                _sb.Length = 0;
            }

            Write("GET ");
            Write(uri.Uri.PathAndQuery);
            return select;
        }

        private static bool IsSingleRequest(Expression expression)
        {
            if (expression == null) return false;

            var binary = expression as BinaryExpression;
            if (binary == null) return false;

            var column = binary.Left as ColumnExpression;
            var named = binary.Right as NamedValueExpression;
            if (column == null || named == null || column.Name != "Id") return false;

            return true;
        }

        protected override Expression VisitSource(Expression source)
        {
            Visit(source);
            return source;
        }

        protected override Expression VisitJoin(JoinExpression join)
        {
            //TODO
            return join;
        }

        protected override Expression VisitAggregate(AggregateExpression aggregate)
        {
            //TODO
            return aggregate;
        }

        protected override Expression VisitIsNull(IsNullExpression isnull)
        {
            //TODO
            return isnull;
        }

        protected override Expression VisitBetween(BetweenExpression between)
        {
            VisitValue(between.Expression);
            Write(" between ");
            VisitValue(between.Lower);
            Write(" and ");
            VisitValue(between.Upper);
            return between;
        }

        protected override Expression VisitRowNumber(RowNumberExpression rowNumber)
        {
            throw new NotSupportedException();
        }

        protected override Expression VisitScalar(ScalarExpression subquery)
        {
            //TODO
            return subquery;
        }

        protected override Expression VisitExists(ExistsExpression exists)
        {
            //TODO
            return exists;
        }

        protected override Expression VisitIn(InExpression @in)
        {
            if (@in.Values != null)
            {
                if (@in.Values.Count == 0)
                {
                    Write("0 ne 0");
                }
                else
                {
                    VisitValue(@in.Expression);
                    Write(" in (");
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
                Write(" in (");
                Visit(@in.Select);
                Write(")");
            }
            return @in;
        }

        protected override Expression VisitNamedValue(NamedValueExpression value)
        {
            Visit(value.Value);
            return value;
        }

        protected override Expression VisitInsert(InsertCommand insert)
        {
            //TODO - insert.Assignments
            var uri = new SDataUri();

            if (insert.Table != null)
            {
                VisitSource(insert.Table);
                uri.AppendPath(_sb.ToString());
                _sb.Length = 0;
            }

            Write("POST ");
            Write(uri.Uri.PathAndQuery);
            return insert;
        }

        protected override Expression VisitUpdate(UpdateCommand update)
        {
            //TODO - update.Assignments
            var uri = new SDataUri();
            var where = update.Where;

            if (update.Table != null)
            {
                VisitSource(update.Table);
                var text = _sb.ToString();
                _sb.Length = 0;
                string predicate;

                if (IsSingleRequest(where))
                {
                    Visit(((BinaryExpression) where).Right);
                    predicate = _sb.ToString();
                    _sb.Length = 0;
                    where = null;
                }
                else
                {
                    predicate = null;
                }

                uri.AppendPath(new UriPathSegment(text, predicate));
            }

            if (where != null)
            {
                Visit(update.Where);
                uri.Where = _sb.ToString();
                _sb.Length = 0;
            }

            Write("PUT ");
            Write(uri.Uri.PathAndQuery);
            return update;
        }

        protected override Expression VisitDelete(DeleteCommand delete)
        {
            var uri = new SDataUri();
            var where = delete.Where;

            if (delete.Table != null)
            {
                VisitSource(delete.Table);
                var text = _sb.ToString();
                _sb.Length = 0;
                string predicate;

                if (IsSingleRequest(where))
                {
                    Visit(((BinaryExpression) where).Right);
                    predicate = _sb.ToString();
                    _sb.Length = 0;
                    where = null;
                }
                else
                {
                    predicate = null;
                }

                uri.AppendPath(new UriPathSegment(text, predicate));
            }

            if (where != null)
            {
                Visit(delete.Where);
                uri.Where = _sb.ToString();
                _sb.Length = 0;
            }

            Write("DELETE ");
            Write(uri.Uri.PathAndQuery);
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
            //TODO
            return vex;
        }

        protected override Expression VisitFunction(FunctionExpression func)
        {
            //TODO
            return func;
        }

        protected override ReadOnlyCollection<ColumnDeclaration> VisitColumnDeclarations(ReadOnlyCollection<ColumnDeclaration> columns)
        {
            var first = true;
            foreach (var column in columns)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    Write(",");
                }
                Visit(column.Expression);
            }
            return columns;
        }

        protected override ReadOnlyCollection<OrderExpression> VisitOrderBy(ReadOnlyCollection<OrderExpression> orders)
        {
            var first = true;
            foreach (var order in orders)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    Write(",");
                }
                Visit(order.Expression);
            }
            return orders;
        }

        protected override Expression VisitTable(TableExpression table)
        {
            Write(table.Name);
            return table;
        }
    }
}