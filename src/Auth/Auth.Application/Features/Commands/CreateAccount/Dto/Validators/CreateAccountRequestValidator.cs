using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;
using FluentValidation.Internal;

namespace Auth.Application.Features.Commands.CreateAccount.Dto.Validators
{
    public class CreateAccountRequestValidator : BaseAbstractValidator<CreateAccountCommand>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).MinimumLength(6);

            RuleFor(x => x.A).NotNull().SetValidator(new AValidator());
        }
    }


    public class AValidator : BaseAbstractValidator<A>
    {
        public AValidator()
        {
            RuleFor(x => x.Test).NotEmpty();
        }
    }
    
    public abstract class BaseAbstractValidator<T> : AbstractValidator<T>
    {
        const string CULTURE_RU = "ru";

        public BaseAbstractValidator()
        {
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(CULTURE_RU);
            ValidatorOptions.Global.PropertyNameResolver = CamelCasePropertyNameResolver.ResolvePropertyName;
        }
    }
    
    public class CamelCasePropertyNameResolver
    {
        public static string ResolvePropertyName(Type type, MemberInfo memberInfo, LambdaExpression expression)
        {
            return ToCamelCase(DefaultPropertyNameResolver(type, memberInfo, expression));
        }

        private static string DefaultPropertyNameResolver(Type type, MemberInfo memberInfo, LambdaExpression expression)
        {
            if (expression != null)
            {
                var chain = PropertyChain.FromExpression(expression);
                if (chain.Count > 0) return chain.ToString();
            }

            if (memberInfo != null)
            {
                return memberInfo.Name;
            }

            return null;
        }

        private static string ToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
            {
                return s;
            }

            var chars = s.ToCharArray();

            for (var i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                {
                    break;
                }

                var hasNext = (i + 1 < chars.Length);
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    break;
                }

                chars[i] = char.ToLower(chars[i], CultureInfo.InvariantCulture);
            }

            return new string(chars);
        }
    }
}