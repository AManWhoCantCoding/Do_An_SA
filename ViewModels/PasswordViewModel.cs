using System.ComponentModel.DataAnnotations;

namespace DocDocGo.ViewModels
{
    public class PasswordViewModel
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
