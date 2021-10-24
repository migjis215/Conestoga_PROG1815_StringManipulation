/*
 * Program ID: Assignment2
 * 
 * Purpose: To create a form that validates input values and manipulates strings 
 *          to reformat input fields
 * 
 * Revision History: 
 *      Jisung Kim, 2021.02.22: Created
 */
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Assignment2
{
    public partial class JKGymMemberManagement : Form
    {
        bool isFocused = false;
        bool isValidValue = true;
        
        public JKGymMemberManagement()
        {
            InitializeComponent();
        }

        // Remove text in message label when the program runs
        // Make the date in the Date Joined invisible because it is before the user selects the date
        // Set the Value and MaxDate of the DateTimePicker to Today
        private void JKGymMemberManagement_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            dtpDateJoined.CustomFormat = " ";
            dtpDateJoined.Value = DateTime.Today;
            dtpDateJoined.MaxDate = DateTime.Today;
        }

        // Focus the first field of the errors
        private void FocusToInvalidFirstField(TextBox field)
        {
            if (lblMessage.Text != "" && !isFocused)
            {
                field.Focus();
                isFocused = true;
            }
        }

        // Check that it contains at least three letters
        private string CheckMailingAddressAndTown(string value, string boxName)
        {
            value = JKValidation.JKCapitalize(value);

            if (value != "")
            {
                Regex pattern = new Regex(@"^([^A-Za-z]*[A-Za-z]+){3,}[^A-Za-z]*$");
                Match match = pattern.Match(value);

                if (!match.Success)
                {
                    lblMessage.Text += "The " + boxName + " entered is not valid. It must contain at least three letters.\n";
                }
            }

            return value;
        }

        // Fill in the fields with valid values
        private void btnPrefill_Click(object sender, EventArgs e)
        {
            txtFullName.Text = "first lAST";
            txtMemberGoal.Text = "To Run 126 mileS; in 2 hours. Perform 300 situps, and Squat 100 pOUNDS, by January of 2021.";
            txtHomePhone.Text = "123 456 7890";
            txtWorkPhone.Text = "";
            txtCellPhone.Text = "123.456.7890";
            txtEmail.Text = "Abc@mail.com";
            dtpDateJoined.CustomFormat = "yyyy MMM dd";
            txtMemberCode.Text = "a555a5aa5";
            txtMailingAddress.Text = "100 my street";
            txtTown.Text = "waterloo";
            txtCountry.Text = "canada";
            txtProvinceCode.Text = "on";
            txtPostalCode.Text = "a33aa";
        }


        // Validate each field and display error messages
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            isFocused = false;
            dtpDateJoined.MaxDate = DateTime.Today;

            // Full Name
            // Check that two or more words are entered
            txtFullName.Text = JKValidation.JKCapitalize(txtFullName.Text);

            if (txtFullName.Text == "")
            {
                lblMessage.Text += "Please enter a 'Full Name.'\n";
            }
            else
            {
                if (!txtFullName.Text.Contains(" "))
                {
                    lblMessage.Text += "Please enter a full name separated by spaces.\n";
                }
            }

            FocusToInvalidFirstField(txtFullName);

            // Member Goal
            // Remove all punctuation characters except the period
            txtMemberGoal.Text = JKValidation.JKCapitalize(txtMemberGoal.Text);

            if (txtMemberGoal.Text == "")
            {
                lblMessage.Text += "Please enter a 'Member Goal.'\n";

                FocusToInvalidFirstField(txtMemberGoal);
            }
            else
            {
                Regex punctuationExceptPeriod = new Regex(@"[^A-Za-z0-9 \.]");
                txtMemberGoal.Text = punctuationExceptPeriod.Replace(txtMemberGoal.Text, "");
            }

            // Home Phone, Work Phone, Cell Phone
            // Check that at least one phone number is entered
            // Validate the entered phone numbers
            if (string.IsNullOrWhiteSpace(txtHomePhone.Text) && 
                string.IsNullOrWhiteSpace(txtWorkPhone.Text) && 
                string.IsNullOrWhiteSpace(txtCellPhone.Text))
            {
                lblMessage.Text += "Please enter at least one phone number.\n";

                FocusToInvalidFirstField(txtHomePhone);
            }
            else
            {
                TextBox[] phoneTextBox = new TextBox[] { txtHomePhone, txtWorkPhone, txtCellPhone };
                Label[] phoneLabel = new Label[] { lblHomePhone, lblWorkPhone, lblCellPhon };

                for (int i = 0; i < 3; i++)
                {
                    if (!string.IsNullOrWhiteSpace(phoneTextBox[i].Text))
                    {
                        isValidValue = JKValidation.JKPhoneNumberValidation(phoneTextBox[i].Text);
                        if (!isValidValue)
                        {
                            lblMessage.Text += "The '" + phoneLabel[i].Text + "' number is not valid. Please re-enter. e.g. 123 456 7890 or 123.456.7890\n";

                            FocusToInvalidFirstField(phoneTextBox[i]);
                        }
                    }
                }
            }

            // E-mail
            // Validate if an email is entered
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                try
                {
                    MailAddress email = new MailAddress(txtEmail.Text);
                    txtEmail.Text = txtEmail.Text.ToLower();
                }
                catch (Exception)
                {
                    lblMessage.Text += "The e-mail address entered is not valid. Please re-enter.\n";

                    FocusToInvalidFirstField(txtEmail);
                }
            }

            // Member Code
            // Validate the member code            
            txtMemberCode.Text = txtMemberCode.Text.Trim();
            isValidValue = JKValidation.JKMemberCodeValidation(txtMemberCode.Text);

            if (isValidValue)
            {
                if (string.IsNullOrEmpty(txtMemberCode.Text))
                {
                    lblMessage.Text += "Please enter a 'Member Code.'\n";
                }
                else
                {
                    txtMemberCode.Text = txtMemberCode.Text.ToUpper();
                }
            }
            else
            {
                lblMessage.Text += "The member code entered is not valid. It must be 9 characters with 5 numbers and 4 letters in any order.\n";
            }

            FocusToInvalidFirstField(txtMemberCode);

            // Mailing Address
            // Validate the mailing address
            txtMailingAddress.Text = CheckMailingAddressAndTown(txtMailingAddress.Text, "mailing address");
            FocusToInvalidFirstField(txtMailingAddress);

            // Town
            // Validate the town
            txtTown.Text = CheckMailingAddressAndTown(txtTown.Text, "town");
            FocusToInvalidFirstField(txtTown);

            // Country
            // Validate if a country is entered
            if (!string.IsNullOrWhiteSpace(txtCountry.Text))
            {
                txtCountry.Text = txtCountry.Text.ToUpper();
            }

            // Province Code
            // Validate the province code
            if (!string.IsNullOrWhiteSpace(txtProvinceCode.Text))
            {
                Regex pattern = new Regex(@"^[A-Za-z]{2}$");
                Match match = pattern.Match(txtProvinceCode.Text);

                if (match.Success)
                {
                    txtProvinceCode.Text = txtProvinceCode.Text.ToUpper();
                }
                else
                {
                    lblMessage.Text += "The province code entered is not valid. It must be exactly two letters.\n";

                    FocusToInvalidFirstField(txtProvinceCode);
                }
            }

            // Empty E-mail value
            // If 'E-mail' is not entered, Check 'Mailing Address', 'Town' and 'Province Code' are all entered
            TextBox[] addressTextBoxes = new TextBox[] { txtMailingAddress, txtTown, txtProvinceCode };

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                for (int i = 0; i < 3; i++)
                {
                    if (addressTextBoxes[i].Text == "")
                    {
                        lblMessage.Text += "If the 'E-mail' is not entered, the 'Mailing Address', 'Town', and 'Province Code' must be entered.\n";

                        FocusToInvalidFirstField(addressTextBoxes[i]);

                        break;
                    }
                }
            }

            // Postal Code
            // Validate the postal code
            string postalCode = txtPostalCode.Text.Trim();
            isValidValue = JKValidation.JKUKPostalValidation(ref postalCode);
            
            if (isValidValue)
            { 
                txtPostalCode.Text = postalCode;
                if (string.IsNullOrEmpty(txtPostalCode.Text))
                {
                    lblMessage.Text += "Please enter a 'Postal Code.'\n";
                }
            }
            else
            {
                lblMessage.Text += "The postal code entered is not valid. Please re-entered.\n";
            }

            FocusToInvalidFirstField(txtPostalCode);
        }

        // Change the format so that the date is visible when the user attempts to select a date
        private void dtpDateJoined_DropDown(object sender, EventArgs e)
        {
            dtpDateJoined.CustomFormat = "yyyy MMM dd";
        }

        // Clear all fields
        private void btnClear_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            txtFullName.Text = "";
            txtMemberGoal.Text = "";
            txtHomePhone.Text = "";
            txtWorkPhone.Text = "";
            txtCellPhone.Text = "";
            txtEmail.Text = "";
            dtpDateJoined.CustomFormat = " ";
            txtMemberCode.Text = "";
            txtMailingAddress.Text = "";
            txtTown.Text = "";
            txtCountry.Text = "";
            txtProvinceCode.Text = "";
            txtPostalCode.Text = "";
        }

        // Exit the program
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
