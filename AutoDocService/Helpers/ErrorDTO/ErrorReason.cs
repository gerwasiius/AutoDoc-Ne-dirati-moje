using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text;

namespace AutoDocService.Helpers.ErrorDTO
{
    /// <summary>
    /// Additional info about caused error.
    /// </summary>
    public partial class ErrorReason : IEquatable<ErrorReason>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorReason" /> class.
        /// </summary>
        /// <param name="code">code (required).</param>
        /// <param name="severity">severity.</param>
        /// <param name="message">Human-readable message in user-requested language. (required).</param>
        /// <param name="path">The path of the problematic field which causes the error..</param>
        public ErrorReason(string code = default(string), SeverityType severity = default(SeverityType), string message = default(string), string path = default(string))
        {
            // to ensure "code" is required (not null)
            if (code == null)
            {
                throw new InvalidDataException("code is a required property for ErrorReason and cannot be null");
            }
            else
            {
                this.Code = code;
            }
            // to ensure "message" is required (not null)
            if (message == null)
            {
                throw new InvalidDataException("message is a required property for ErrorReason and cannot be null");
            }
            else
            {
                this.Message = message;
            }
            this.Severity = severity;
            this.Path = path;
        }

        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or Sets Severity
        /// </summary>

        [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
        public SeverityType Severity { get; set; }

        /// <summary>
        /// Human-readable message in user-requested language.
        /// </summary>
        /// <value>Human-readable message in user-requested language.</value>
        public string Message { get; set; }

        /// <summary>
        /// The path of the problematic field which causes the error.
        /// </summary>
        /// <value>The path of the problematic field which causes the error.</value>
        public string Path { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ErrorReason {\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Severity: ").Append(Severity).Append("\n");
            sb.Append("  Message: ").Append(Message).Append("\n");
            sb.Append("  Path: ").Append(Path).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as ErrorReason);
        }

        /// <summary>
        /// Returns true if ErrorReason instances are equal
        /// </summary>
        /// <param name="input">Instance of ErrorReason to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ErrorReason input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Code == input.Code ||
                    (this.Code != null &&
                    this.Code.Equals(input.Code))
                ) &&
                (
                    this.Severity == input.Severity ||
                    (this.Severity != null &&
                    this.Severity.Equals(input.Severity))
                ) &&
                (
                    this.Message == input.Message ||
                    (this.Message != null &&
                    this.Message.Equals(input.Message))
                ) &&
                (
                    this.Path == input.Path ||
                    (this.Path != null &&
                    this.Path.Equals(input.Path))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Code != null)
                    hashCode = hashCode * 59 + this.Code.GetHashCode();
                if (this.Severity != null)
                    hashCode = hashCode * 59 + this.Severity.GetHashCode();
                if (this.Message != null)
                    hashCode = hashCode * 59 + this.Message.GetHashCode();
                if (this.Path != null)
                    hashCode = hashCode * 59 + this.Path.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
