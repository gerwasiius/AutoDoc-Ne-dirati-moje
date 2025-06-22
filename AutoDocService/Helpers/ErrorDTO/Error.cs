using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutoDocService.Helpers.ErrorDTO
{
    /// <summary>
    /// Object for error response
    /// </summary>
    public partial class Error : IEquatable<Error>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Error" /> class.
        /// </summary>
        /// <param name="errorId">Unique UUID to identify specific error..</param>
        /// <param name="requestId">Unique UUID of specific request. Value shoud be obtained from X-Request-Id header. (required).</param>
        /// <param name="correlationId">Unique UUID for batch of requests. Value shoud be obtained from X-Correlation-Id header..</param>
        /// <param name="status">HTTP status code. If there is in use different protocol than HTTP we should map error to this protocol.  (required).</param>
        /// <param name="reasons">reasons.</param>
        public Error(string errorId = default(string), int? status = default(int?), List<ErrorReason> reasons = default(List<ErrorReason>))
        {
            // to ensure "requestId" is required (not null)

            // to ensure "status" is required (not null)
            this.Status = status;
            this.ErrorId = errorId;
            this.Reasons = reasons;
        }

        /// <summary>
        /// Unique UUID to identify specific error.
        /// </summary>
        /// <value>Unique UUID to identify specific error.</value>
        public string ErrorId { get; set; }

        /// <summary>
        /// HTTP status code. If there is in use different protocol than HTTP we should map error to this protocol. 
        /// </summary>
        /// <value>HTTP status code. If there is in use different protocol than HTTP we should map error to this protocol. </value>
        public int? Status { get; set; }

        /// <summary>
        /// Gets or Sets Reasons
        /// </summary>
        public List<ErrorReason> Reasons { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Error {\n");
            sb.Append("  ErrorId: ").Append(ErrorId).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  Reasons: ").Append(Reasons).Append("\n");
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
            return this.Equals(input as Error);
        }

        /// <summary>
        /// Returns true if Error instances are equal
        /// </summary>
        /// <param name="input">Instance of Error to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Error input)
        {
            if (input == null)
                return false;

            return
                (
                    this.ErrorId == input.ErrorId ||
                    (this.ErrorId != null &&
                    this.ErrorId.Equals(input.ErrorId))
                ) &&
                (
                    this.Status == input.Status ||
                    (this.Status != null &&
                    this.Status.Equals(input.Status))
                ) &&
                (
                    this.Reasons == input.Reasons ||
                    this.Reasons != null &&
                    input.Reasons != null &&
                    this.Reasons.SequenceEqual(input.Reasons)
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
                if (this.ErrorId != null)
                    hashCode = hashCode * 59 + this.ErrorId.GetHashCode();
                if (this.Status != null)
                    hashCode = hashCode * 59 + this.Status.GetHashCode();
                if (this.Reasons != null)
                    hashCode = hashCode * 59 + this.Reasons.GetHashCode();
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
