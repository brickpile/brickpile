using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace BrickPile.UI.Areas.UI.Models
{

	#region Models

	public class ChangePasswordModel
	{
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Current password")]
		public string OldPassword { get; set; }

		[Required]
		[ValidatePasswordLength]
		[DataType(DataType.Password)]
		[Display(Name = "New password")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm new password")]
		[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public class LogOnModel
	{
		[Required]
		[Display(Name = "Username")]
		public string UserName { get; set; }

        [Required(ErrorMessage = "well, password")]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}


	public class RegisterModel
	{


		[Required(ErrorMessage = "Required!")]
		[Display(Name = "Username")]
		public string UserName { get; set; }

        [Required(ErrorMessage = "Required!")]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email address")]
		public string Email { get; set; }

        [Required(ErrorMessage = "Required!")]
		[ValidatePasswordLength(ErrorMessage = "To short i'm afraid!")]
		[DataType(DataType.Password)]
		[Display(Name = "Password...")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "...and again")]
		[Compare("Password", ErrorMessage = "The passwords do not match!")]
		public string ConfirmPassword { get; set; }
	}
	#endregion

	#region Validation

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
	public sealed class CompareAttribute : ValidationAttribute, IClientValidatable
	{
		private const string _defaultErrorMessage = "'{0}' and '{1}' do not match.";
		private readonly object _typeId = new object();

		public CompareAttribute(string confirmProperty)
			: base(_defaultErrorMessage)
		{
			ConfirmProperty = confirmProperty;
		}

		public string ConfirmProperty { get; private set; }

		public override object TypeId
		{
			get
			{
				return _typeId;
			}
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentCulture, ErrorMessageString,
				name, ConfirmProperty);
		}

		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			var confirmValue = context.ObjectType.GetProperty(ConfirmProperty).GetValue(context.ObjectInstance, null);
			if (!Equals(value, confirmValue))
			{
				return new ValidationResult(FormatErrorMessage(context.DisplayName));
			}
			return null;
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			return new[]{
                new ModelClientValidationEqualToRule(FormatErrorMessage(metadata.GetDisplayName()), ConfirmProperty)
            };
		}
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class ValidatePasswordLengthAttribute : ValidationAttribute, IClientValidatable
	{
		private const string _defaultErrorMessage = "'{0}' must be at least {1} characters long.";
		private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

		public ValidatePasswordLengthAttribute()
			: base(_defaultErrorMessage)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentCulture, ErrorMessageString,
				name, _minCharacters);
		}

		public override bool IsValid(object value)
		{
			string valueAsString = value as string;
			return (valueAsString != null && valueAsString.Length >= _minCharacters);
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			return new[]{
                new ModelClientValidationStringLengthRule(FormatErrorMessage(metadata.GetDisplayName()), _minCharacters, int.MaxValue)
            };
		}
	}
	#endregion

}
