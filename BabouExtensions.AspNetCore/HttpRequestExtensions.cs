using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BabouExtensions.AspNetCore
{
    /// <summary>
    /// Extensions for HttpRequest in AspNetCore
    /// </summary>
    public static class HttpRequestExtensions
    {

        /// <summary>
        /// Retrieve the raw body as a string from the Request.Body stream
        /// </summary>
        /// <param name="request">Request instance to apply to</param>
        /// <param name="encoding">Optional - Encoding, defaults to UTF8</param>
        /// <returns></returns>
        public static async Task<string> GetRawBodyStringAsync(this HttpRequest request, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            using var reader = new StreamReader(request.Body, encoding);
            return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// Retrieve the raw body as a string from the Request.Body stream. Call Request.EnableBuffering() before using this method.
        /// </summary>
        /// <param name="request">Request instance to apply to</param>
        /// <param name="encoding">Optional - Encoding, defaults to UTF8</param>
        /// <param name="bufferSize">Option - Defaults to -1</param>
        /// <param name="leaveOpen">Option - Set to true if you need to use the Request Body again</param>
        /// <returns></returns>
        public static async Task<string> GetRawBodyStringAsyncWithOptions(this HttpRequest request, Encoding encoding = null, int? bufferSize = null, bool? leaveOpen = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            if (bufferSize == null)
                bufferSize = -1;

            if (leaveOpen == null)
                leaveOpen = false;

            string requestBody;

            using (var reader = new StreamReader(request.Body, encoding: encoding, detectEncodingFromByteOrderMarks: false, bufferSize: bufferSize.Value, leaveOpen: leaveOpen.Value))
            {
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }
            return requestBody;
        }

        /// <summary>
        /// Retrieves the raw body as a byte array from the Request.Body stream
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<byte[]> GetRawBodyBytesAsync(this HttpRequest request)
        {
            await using var ms = new MemoryStream(2048);
            await request.Body.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
