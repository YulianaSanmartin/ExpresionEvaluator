namespace Evaluator.Logic;

public class FunctionEvaluator
{
    public static double Evalute(string infix)
    {
        var postfix = ToPostfix(infix);
        return Calculate(postfix);
    }

    private static double Calculate(string postfix)
    {
        var stack = new Stack<double>();
        var number = string.Empty;

        foreach (var item in postfix)
        {
            if (char.IsDigit(item) || item == '.')
            {
                number += item;
            }
            else if (item == ' ')
            {
                if (!string.IsNullOrEmpty(number))
                {
                    stack.Push(double.Parse(number, System.Globalization.CultureInfo.InvariantCulture));
                    number = string.Empty;
                }
            }
            else if (IsOperator(item))
            {
                var operator2 = stack.Pop();
                var operator1 = stack.Pop();
                stack.Push(Result(operator1, item, operator2));
            }
        }

        if (!string.IsNullOrEmpty(number))
        {
            stack.Push(double.Parse(number, System.Globalization.CultureInfo.InvariantCulture));
        }

        return stack.Pop();
    }

    private static double Result(double operator1, char item, double operator2)
    {
        return item switch
        {
            '+' => operator1 + operator2,
            '-' => operator1 - operator2,
            '*' => operator1 * operator2,
            '/' => operator1 / operator2,
            '^' => Math.Pow(operator1, operator2),
            _ => throw new Exception("Invalid expression"),
        };
    }

    private static string ToPostfix(string infix)
    {
        var stack = new Stack<char>();
        var postfix = string.Empty;
        var number = string.Empty;

        foreach (var item in infix)
        {
            if (char.IsDigit(item) || item == '.')
            {
                number += item;
            }
            else
            {
                if (number.Length > 0)
                {
                    postfix += number + " ";
                    number = string.Empty;
                }

                if (IsOperator(item))
                {
                    if (item == '(')
                    {
                        stack.Push(item);
                    }
                    else if (item == ')')
                    {
                        while (stack.Count > 0 && stack.Peek() != '(')
                        {
                            postfix += stack.Pop() + " ";
                        }
                        if (stack.Count > 0 && stack.Peek() == '(')
                            stack.Pop();
                    }
                    else
                    {
                        while (stack.Count > 0 && PriorityExpression(item) <= PriorityStack(stack.Peek()))
                        {
                            postfix += stack.Pop() + " ";
                        }
                        stack.Push(item);
                    }
                }
                else if (item != ' ')
                {
                    throw new Exception("Invalid character in expression");
                }
            }
        }

        if (number.Length > 0)
        {
            postfix += number + " ";
        }

        while (stack.Count > 0)
        {
            postfix += stack.Pop() + " ";
        }

        return postfix;
    }

    private static int PriorityStack(char item)
    {
        return item switch
        {
            '^' => 3,
            '*' => 2,
            '/' => 2,
            '+' => 1,
            '-' => 1,
            '(' => 0,
            _ => throw new Exception("Invalid expression."),
        };
    }

    private static int PriorityExpression(char item)
    {
        return item switch
        {
            '^' => 4,
            '*' => 2,
            '/' => 2,
            '+' => 1,
            '-' => 1,
            '(' => 5,
            _ => throw new Exception("Invalid expression."),
        };
    }

    private static bool IsOperator(char item) => "()^*/+-".IndexOf(item) >= 0;
}
