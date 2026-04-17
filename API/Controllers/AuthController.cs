using BookingTrain.Application.DTOs;
using BookingTrain.Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingTrain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _authService.RegisterAsync(request);
                return CreatedAtAction(nameof(Register), new { email = response.Email }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ: " + ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ: " + ex.Message });
            }
        }

        // POST api/auth/logout
        /// <summary>
        /// Logout phía client – chỉ cần xóa token ở client (JWT stateless).
        /// Endpoint này chỉ để UI gọi tường minh, không làm gì phía server.
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // JWT là stateless nên server không cần làm gì thêm.
            // Client cần xóa token khỏi storage (localStorage / Cookie).
            return Ok(new { message = "Đăng xuất thành công. Vui lòng xóa token ở phía client." });
        }

        // GET api/auth/me
        /// <summary>Lấy thông tin của người dùng đang đăng nhập từ JWT.</summary>
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId    = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                         ?? User.FindFirst("sub")?.Value;
            var email     = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                         ?? User.FindFirst("email")?.Value;
            var name      = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var role      = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new { userId, name, email, role });
        }
    }
}
