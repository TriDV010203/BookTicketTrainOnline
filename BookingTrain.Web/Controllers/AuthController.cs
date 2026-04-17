using System.Net.Http.Json;
using System.Text.Json;
using BookingTrain.Web.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BookingTrain.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AuthController> _logger;

        // Session keys
        private const string SessionToken    = "JWT_Token";
        private const string SessionUserName = "User_Name";
        private const string SessionUserEmail= "User_Email";
        private const string SessionUserRole = "User_Role";

        public AuthController(IHttpClientFactory httpClientFactory, ILogger<AuthController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // ── LOGIN ──────────────────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // Nếu đã đăng nhập → về trang chủ
            if (HttpContext.Session.GetString(SessionToken) != null)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(model);

            try
            {
                var client = _httpClientFactory.CreateClient("BookingTrainAPI");
                var response = await client.PostAsJsonAsync("/api/auth/login", new
                {
                    email    = model.Email,
                    password = model.Password
                });

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    if (data != null)
                    {
                        // Lưu vào Session
                        HttpContext.Session.SetString(SessionToken,     data.Token);
                        HttpContext.Session.SetString(SessionUserName,  data.Name);
                        HttpContext.Session.SetString(SessionUserEmail, data.Email);
                        HttpContext.Session.SetString(SessionUserRole,  data.Role);

                        TempData["SuccessMsg"] = $"Chào mừng trở lại, {data.Name}! 🎉";

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var errJson = await response.Content.ReadAsStringAsync();
                    string errMsg = "Email hoặc mật khẩu không đúng.";
                    try
                    {
                        var doc = JsonDocument.Parse(errJson);
                        if (doc.RootElement.TryGetProperty("message", out var m))
                            errMsg = m.GetString() ?? errMsg;
                    }
                    catch { /* giữ errMsg mặc định */ }
                    ModelState.AddModelError(string.Empty, errMsg);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gọi API đăng nhập");
                ModelState.AddModelError(string.Empty, "Không thể kết nối đến máy chủ. Vui lòng thử lại.");
            }

            return View(model);
        }

        // ── REGISTER ───────────────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString(SessionToken) != null)
                return RedirectToAction("Index", "Home");
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var client = _httpClientFactory.CreateClient("BookingTrainAPI");
                var response = await client.PostAsJsonAsync("/api/auth/register", new
                {
                    name            = model.Name,
                    email           = model.Email,
                    password        = model.Password,
                    confirmPassword = model.ConfirmPassword
                });

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    if (data != null)
                    {
                        HttpContext.Session.SetString(SessionToken,     data.Token);
                        HttpContext.Session.SetString(SessionUserName,  data.Name);
                        HttpContext.Session.SetString(SessionUserEmail, data.Email);
                        HttpContext.Session.SetString(SessionUserRole,  data.Role);

                        TempData["SuccessMsg"] = $"Tạo tài khoản thành công! Chào mừng bạn, {data.Name} 🎉";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var errJson = await response.Content.ReadAsStringAsync();
                    string errMsg = "Đăng ký thất bại. Vui lòng thử lại.";
                    try
                    {
                        var doc = JsonDocument.Parse(errJson);
                        if (doc.RootElement.TryGetProperty("message", out var m))
                            errMsg = m.GetString() ?? errMsg;
                    }
                    catch { /* giữ errMsg mặc định */ }
                    ModelState.AddModelError(string.Empty, errMsg);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gọi API đăng ký");
                ModelState.AddModelError(string.Empty, "Không thể kết nối đến máy chủ. Vui lòng thử lại.");
            }

            return View(model);
        }

        // ── LOGOUT ─────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMsg"] = "Đã đăng xuất thành công!";
            return RedirectToAction("Login", "Auth");
        }
    }
}
