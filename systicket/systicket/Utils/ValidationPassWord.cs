using System;
using System.Text;

namespace systicket.Controllers
{
    public class ValidationPassWord
    {
        string passwordApi = string.Empty;
        string passwordBco = string.Empty;
        public ValidationPassWord(){}

        #region Constructor
        public bool Validation(string password1, string password2)
        {
            passwordApi = password1;
            passwordBco = password2;
            return isValid();
        }

        public string Validation(string key)
        {
            passwordApi = key;           
            return incriptKey();
        }
        #endregion Constructor

        #region Valida Password
        bool isValid()
        {
            string Pass = "";
            for (int i = 0; i < passwordApi.Length; i++)
            {
                Pass += Incript(passwordApi[i].ToString().ToUpper());
            }

            var code = Encoding.Unicode.GetBytes(Pass ?? "");           

            var PassIncrip = Convert.ToBase64String(code);

            return passwordBco == PassIncrip ? true:false;
        }

        string incriptKey()
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
            string unic;

            switch (p)
            {
                case "A":
                     unic = "L";
                    break;
                case "B":
                     unic = "C";
                    break;
                case "C":
                     unic = "J";
                    break;
                case "D":
                     unic = "N";
                    break;
                case "E":
                     unic = "F";
                    break;
                case "F":
                     unic = "K";
                    break;
                case "G":
                     unic = "A";
                    break;
                case "H":
                     unic = "P";
                    break;
                case "I":
                     unic = "E";
                    break;
                case "J":
                     unic = "M";
                    break;
                case "K":
                     unic = "I";
                    break;
                case "L":
                     unic = "O";
                    break;
                case "M":
                     unic = "T";
                    break;
                case "N":
                     unic = "B";
                    break;
                case "O":
                     unic = "V";
                    break;
                case "P":
                     unic = "H";
                    break;
                case "Q":
                     unic = "U";
                    break;
                case "R":
                     unic = "D";
                    break;
                case "S":
                     unic = "Q";
                    break;
                case "T":
                     unic = "X";
                    break;
                case "U":
                     unic = "R";
                    break;
                case "V":
                     unic = "G";
                    break;
                case "X":
                     unic = "Z";
                    break;
                case "Z":
                     unic = "S";
                    break;
                case "0":
                    unic = "1";
                    break;
                case "1":
                     unic = "2";
                    break;
                case "2":
                     unic = "3";
                    break;
                case "3":
                    unic = "4";
                    break;
                case "4":
                    unic = "5";
                    break;
                case "5":
                    unic = "6";
                    break;
                case "6":
                    unic = "7";
                    break;
                case "7":
                    unic = "8";
                    break;
                case "8":
                    unic = "9";
                    break;
                case "9":
                    unic = "0";
                    break;
                default:
                    unic = "";
                    break;
            }
            return unic;
        }

        #endregion Chage Caracter

    }
}
