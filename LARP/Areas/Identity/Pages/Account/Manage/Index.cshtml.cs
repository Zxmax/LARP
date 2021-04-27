using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LARP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LARP.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ScriptUser> _userManager;
        private readonly SignInManager<ScriptUser> _signInManager;

        public IndexModel(
            UserManager<ScriptUser> userManager,
            SignInManager<ScriptUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [Display(Name = "用户名")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "电话号码")]
            public string PhoneNumber { get; set; }
            [Display(Name = "昵称")]
            public string NickName { get; set; }
            [Display(Name = "性别")]
            public int Sex { get; set; }
            [Display(Name = "地区")]
            public string Location { get; set; }
            [Display(Name = "头像")]
            public byte[] Avatar { get; set; }
        }

        private async Task LoadAsync(ScriptUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                NickName=user.NickName,
                Sex = user.Sex,
                Location = user.Location,
                Avatar = user.Avatar

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"找不到该用户-- '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"找不到该用户-- '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "更改电话号码异常";
                    return RedirectToPage();
                }
            }

            var nickName = user.NickName;
            var sex = user.Sex;
            var location = user.Location;
            if (Input.NickName != nickName)
            {
                user.NickName = Input.NickName;
                await _userManager.UpdateAsync(user);
            }
            if (Input.Sex != sex)
            {
                user.Sex = Input.Sex;
                await _userManager.UpdateAsync(user);
            }
            if (Input.Location != location)
            {
                user.Location = Input.Location;
                await _userManager.UpdateAsync(user);
            }

            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files.FirstOrDefault();
                await using (var dataStream = new MemoryStream())
                {
                    if (file != null) await file.CopyToAsync(dataStream);
                    user.Avatar = dataStream.ToArray();
                }
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "你的个人资料已经更改";
            return RedirectToPage();
        }
    }
}
