using System.ComponentModel.DataAnnotations;

namespace CandidateHubApi.Attributes
{
    public class TimeIntervalAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(value as string))
                return ValidationResult.Success;

            if (value is string timeInterval)
            {
                // Split the time interval into start and end times
                var times = timeInterval.Split('-');
                if (times.Length != 2)
                {
                    return new ValidationResult("Time interval must be in the format HH:mm-HH:mm.");
                }

                // Parse the start and end times
                if (DateTime.TryParse(times[0], out DateTime startTime) &&
                    DateTime.TryParse(times[1], out DateTime endTime))
                {
                    // Check if the start time is less than or equal to the end time
                    if (startTime <= endTime && startTime.TimeOfDay >= TimeSpan.Zero && endTime.TimeOfDay <= new TimeSpan(23, 59, 59))
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult("Start time must be less than or equal to end time, and both must be within a single day (00:00-23:59).");
                    }
                }
                else
                {
                    return new ValidationResult("Invalid time format. Ensure times are in HH:mm format.");
                }
            }

            return new ValidationResult("Invalid time interval.");
        }
    }

}
