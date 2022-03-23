using System;

namespace Shared.AllConstants
{
    public static class LogMessages
    {
        /// <summary>
        /// {0} Service name, {1} Method name
        /// </summary>
        public static string NullModel = $"{DateTime.UtcNow} - {0}.{1}() received null model for input.";

        /// <summary>
        /// {0} Service name, {1} Method name, {2} id, {3} for model
        /// </summary>
        public static string InvalidId = $"{DateTime.UtcNow} - {0}.{1}() received invalid id: {2}, for {3}.";

        /// <summary>
        /// {0} Service name, {1} Method name, {2} id
        /// </summary>
        public static string NotFound = $"{DateTime.UtcNow} - {0}.{1}() didn't find any mathing elements for id: {2}.";

        /// <summary>
        /// {0} Service name, {1} Method name, {2} for model
        /// </summary>
        public static string NotFoundModel = $"{DateTime.UtcNow} - {0}.{1}() didn't find any mathing elements in database for model: {2}.";

        /// <summary>
        /// {0} Service name, {1} Method name, {2} input name
        /// </summary>
        public static string UniqueName = $"{DateTime.UtcNow} - {0}.{1}() input name: {2} is already used. Unique name is required!";

        /// <summary>
        /// {0} Service name, {1} Method name
        /// </summary>
        public static string InvalidType = $"{DateTime.UtcNow} - {0}.{1}() Unsupported type!";
    }
}
