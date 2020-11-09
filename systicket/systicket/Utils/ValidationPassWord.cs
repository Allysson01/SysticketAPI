using System;
using System.Text;
using systicket.Models;

namespace systicket.Controllers
{
    public class ValidationPassWord
    {
        private string passwordApi = string.Empty;
        private string passwordBco = string.Empty;
        private Login login;

        public ValidationPassWord(Login login)
        {
            this.login = login;
        }

        public ValidationPassWord(){}

        #region Constructor

        public bool ValidationPassword(Login login, string password2)
        {
            this.login = login;
            passwordApi = login.Password;
            passwordBco = password2;
            return IsValid();
        }

        public bool VerificationKey(string password, string password2)
        {
            passwordApi = password;
            passwordBco = password2;
            return IsValid();
        }

        public string ValidationKey(string key)
        {
            passwordApi = key;
            return IncriptKey();
        }

        #endregion Constructor

        #region Valida Password
        private bool IsValid()
        {
            string Pass = "";
            for (int i = 0; i < passwordApi.Length; i++)
            {
                Pass += Incript(passwordApi[i].ToString().ToUpper());
            }

            var code = Encoding.Unicode.GetBytes(Pass ?? "");

            var PassIncrip = Convert.ToBase64String(code);

            return passwordBco == PassIncrip;
        }

        private string IncriptKey()
        {
            string Pass = "";
            for (int i = 0; i < passwordApi.Length; i++)
            {
                Pass += Incript(passwordApi[i].ToString().ToUpper());
            }

            var code = Encoding.Unicode.GetBytes(Pass ?? "");

            string Incrip = Convert.ToBase64String(code);

            return Incrip;
        }

        #endregion Valida Password

        #region Chage Caracter
        string Incript(string p)
        {
            string unic = p switch
            {
                "A" => "L",
                "B" => "C",
                "C" => "J",
                "D" => "N",
                "E" => "F",
                "F" => "K",
                "G" => "A",
                "H" => "P",
                "I" => "E",
                "J" => "M",
                "K" => "I",
                "L" => "O",
                "M" => "T",
                "N" => "B",
                "O" => "V",
                "P" => "H",
                "Q" => "U",
                "R" => "D",
                "S" => "Q",
                "T" => "X",
                "U" => "R",
                "V" => "G",
                "X" => "Z",
                "Y" => "&",
                "Z" => "S",
                "0" => "1",
                "1" => "2",
                "2" => "3",
                "3" => "4",
                "4" => "5",
                "5" => "6",
                "6" => "7",
                "7" => "8",
                "8" => "9",
                "9" => "0",
                _ => "",
            };
            return unic;
        }

        #endregion Chage Caracter

    }
}
