using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Asynchronous.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly ILogger<SampleController> _logger;
        private readonly Random _random;

        public SampleController(ILogger<SampleController> logger)
        {
            _logger = logger;
            _random = new Random();
        }

        /// <summary>
        /// Uses the cancellation token to cancel a request with an exception.
        /// </summary>
        /// <param name="executionTimeInMilliseconds">The time (in milliseconds) to simulate an execution. Minimum value allowed is 0 (zero).</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>Random number.</returns>
        [HttpGet("DoPropagateAbort/{executionTimeInMilliseconds:min(0)}")]
        public async Task<IActionResult> DoPropagateAbort(int executionTimeInMilliseconds, CancellationToken cancellationToken)
        {
            var requestIdentification = Guid.NewGuid();
            _logger.LogInformation($"REQUEST: {requestIdentification} - ********** DoPropagateAbort **********");
            var number = await GetNumber(requestIdentification, executionTimeInMilliseconds, cancellationToken: cancellationToken);

            return Ok(number);
        }

        /// <summary>
        /// Uses the cancellation token to cancel a request gracefully.
        /// </summary>
        /// <param name="executionTimeInMilliseconds">The time (in milliseconds) to simulate an execution. Minimum value allowed is 0 (zero).</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>Random number.</returns>
        [HttpGet("DoPropagateGracefully/{executionTimeInMilliseconds:min(0)}")]
        public async Task<IActionResult> DoPropagateGracefully(int executionTimeInMilliseconds, CancellationToken cancellationToken)
        {
            var requestIdentification = Guid.NewGuid();
            _logger.LogInformation($"REQUEST: {requestIdentification} - ********** DoPropagateGracefully **********");
            var number = await GetNumber(requestIdentification, executionTimeInMilliseconds, true, cancellationToken);

            return Ok(number);
        }

        /// <summary>
        /// Ignores the existing cancellation token as the called method doesn't receive it. The called method continues its execution in background.
        /// </summary>
        /// <param name="executionTimeInMilliseconds">The time (in milliseconds) to simulate an execution. Minimum value allowed is 0 (zero).</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>Random number.</returns>
        [HttpGet("DoNotPropagateToCalled/{executionTimeInMilliseconds:min(0)}")]
        public async Task<IActionResult> DoNotPropagateToCalled(int executionTimeInMilliseconds, CancellationToken cancellationToken)
        {
            var requestIdentification = Guid.NewGuid();
            _logger.LogInformation($"REQUEST: {requestIdentification} - ********** DoNotPropagateToCalled **********");
            var number = await GetNumber(requestIdentification, executionTimeInMilliseconds);

            return Ok(number);
        }

        /// <summary>
        /// There is no cancellation token specified in the controller action, so the called method continues its execution in background.
        /// </summary>
        /// <param name="executionTimeInMilliseconds">The time (in milliseconds) to simulate an execution. Minimum value allowed is 0 (zero).</param>
        /// <returns>Random number.</returns>
        [HttpGet("DoNotPropagate/{executionTimeInMilliseconds:min(0)}")]
        public async Task<IActionResult> DoNotPropagate(int executionTimeInMilliseconds)
        {
            var requestIdentification = Guid.NewGuid();
            _logger.LogInformation($"REQUEST: {requestIdentification} - ********** DoNotPropagate **********");
            var number = await GetNumber(requestIdentification, executionTimeInMilliseconds);

            return Ok(number);
        }

        private async Task<int> GetNumber(Guid requestIdentification, int executionTimeInMilliseconds, bool gracefullyEnd = false, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"REQUEST: {requestIdentification} - Starting GetNumberPropagating...");
            var number = await GenerateNumber(requestIdentification);

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            while (stopWatch.ElapsedMilliseconds < executionTimeInMilliseconds)
            {
                if (gracefullyEnd && cancellationToken.IsCancellationRequested)
                {
                    stopWatch.Stop();

                    _logger.LogInformation($"REQUEST: {requestIdentification} - Cancelled in {stopWatch.ElapsedMilliseconds}ms. Returning {int.MinValue}.");
                    return int.MinValue;
                }
                cancellationToken.ThrowIfCancellationRequested();
                number = await GenerateNumber(requestIdentification);
            }

            stopWatch.Stop();

            _logger.LogInformation($"REQUEST: {requestIdentification} - Ended GetNumberPropagating in {stopWatch.ElapsedMilliseconds}ms. Returning {number}.");
            return number;
        }

        private async Task<int> GenerateNumber(Guid requestIdentification)
        {
            _logger.LogDebug($"REQUEST: {requestIdentification} - Delaying number generation started...");
            await Task.Delay(100);
            _logger.LogDebug($"REQUEST: {requestIdentification} - Delaying number generation completed.");

            var number = _random.Next();
            _logger.LogDebug($"REQUEST: {requestIdentification} - GenerateNumber: {number}");
            return await Task.FromResult(number);
        }
    }
}
