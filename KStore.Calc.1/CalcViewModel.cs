using KStore.Calc._2.Model;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KStore.Calc._2
{
    public class CalcViewModel : BindableBase, INotifyDataErrorInfo
    {
        private ErrorsContainer<string> _errors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            return _errors.GetErrors(propertyName);
        }

        public bool HasErrors
        {
            get { return _errors.HasErrors; }
        }

        public CalcViewModel()
        {
            _errors = new ErrorsContainer<string>(OnErrorsChanged);
            ValidateAllObjects();
        }

        private void OnErrorsChanged([CallerMemberName]string propertyName = null)
        {
            var handler = this.ErrorsChanged;
            if (handler != null)
            {
                handler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        protected void ValidateProperty(object value, [CallerMemberName]string propertyName = null)
        {
            var context = new ValidationContext(this) 
            { 
                MemberName = propertyName 
            };
            var validationErrors = new List<ValidationResult>();
            if (!Validator.TryValidateProperty(value, context, validationErrors))
            {
                this._errors.SetErrors(propertyName, validationErrors.Select(error => error.ErrorMessage));
            }
            else
            {
                this._errors.ClearErrors(propertyName);
            }
        }

        private string _RangeValidationValue;

        [Required(ErrorMessage = "{0} is required.")]
        [Range(0, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public string RangeValidationValue
        {
            get { return _RangeValidationValue; }
            set 
            { 
                this.SetProperty(ref this._RangeValidationValue, value);
                this.ValidateProperty(value);
            }
        }

        private string _MultiByteValidationValue;

        [Required(ErrorMessage = "{0} is required.")]
        [RegularExpression(@"[^\x01-\x7E]+", ErrorMessage = "Value for {0} must be multi-byte-char or half-width kana.")]
        public string MultiByteValidationValue
        {
            get { return _MultiByteValidationValue; }
            set
            {
                this.SetProperty(ref this._MultiByteValidationValue, value);
                this.ValidateProperty(value);
            }
        }

        private string _EmailValidationValue;

        [Required(ErrorMessage = "{0} is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailValidationValue
        {
            get { return _EmailValidationValue; }
            set
            {
                this.SetProperty(ref this._EmailValidationValue, value);
                this.ValidateProperty(value);
            }
        }


        private string _phoneValidationValue;

        [Required(ErrorMessage = "{0} is required.")]
        [Phone(ErrorMessage = "Invalid Phone format.")]
        public string PhoneValidationValue
        {
            get { return _phoneValidationValue; }
            set
            {
                this.SetProperty(ref this._phoneValidationValue, value);
                this.ValidateProperty(value);
            }
        }

        private string _urlValidationValue;

        [Required(ErrorMessage = "{0} is required.")]
        [Url(ErrorMessage = "Invalid Url")]
        public string UrlValidationValue
        {
            get { return _urlValidationValue; }
            set
            {
                this.SetProperty(ref this._urlValidationValue, value);
                this.ValidateProperty(value);
            }
        }

        private string _minLengthValidationValue;

        [Required(ErrorMessage = "{0} is required.")]
        [MinLength(5, ErrorMessage = "Value for {0} must be bigger {1}.")]
        public string MinLengthValidationValue
        {
            get { return _minLengthValidationValue; }
            set
            {
                this.SetProperty(ref this._minLengthValidationValue, value);
                this.ValidateProperty(value);
            }
        }

        private string _maxLengthValidationValue;

        [Required(ErrorMessage = "{0} is required.")]
        [MaxLength(5, ErrorMessage = "Value for {0} must be smaller {1}.")]
        public string MaxLengthValidationValue
        {
            get { return _maxLengthValidationValue; }
            set
            {
                this.SetProperty(ref this._maxLengthValidationValue, value);
                this.ValidateProperty(value);
            }
        }

        private string _fileExtensionsLengthValidationValue;

        [Required(ErrorMessage = "{0} is required.")]
        [FileExtensions( Extensions=".jpg,.xml,.xlsx", ErrorMessage = "Please specify a valid file ({1})")]
        public string FileExtensionsValidationValue
        {
            get { return _fileExtensionsLengthValidationValue; }
            set
            {
                this.SetProperty(ref this._fileExtensionsLengthValidationValue, value);
                this.ValidateProperty(value);
            }
        }

        private string _stringLengthValidationValue;

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public string StringLengthValidationValue
        {
            get { return _stringLengthValidationValue; }
            set
            {
                this.SetProperty(ref this._stringLengthValidationValue, value);
                this.ValidateProperty(value);
            }
        }

        private string _timestampValidationValue;

        [Required(ErrorMessage = "{0} is required.")]
        [Range(typeof(DateTime), "2011/1/1 12:00:00", "2016/1/1 03:00:00", ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public string TimestampValidationValue
        {
            get { return _timestampValidationValue; }
            set
            {
                this.SetProperty(ref this._timestampValidationValue, value);
                this.ValidateProperty(value);
            }
        }



        internal bool ValidateAllObjects()
        {
            if (!this.HasErrors)
            {
                var context = new ValidationContext(this);
                var validationErrors = new List<ValidationResult>();
                if (Validator.TryValidateObject(this, context, validationErrors))
                {
                    return true;
                }

                var errors = validationErrors.Where(_ => _.MemberNames.Any()).GroupBy(_ => _.MemberNames.First());
                foreach (var error in errors)
                {
                    _errors.SetErrors(error.Key, error.Select(_ => _.ErrorMessage));
                }
            }
            return false;
        }
        
    }

 

}
