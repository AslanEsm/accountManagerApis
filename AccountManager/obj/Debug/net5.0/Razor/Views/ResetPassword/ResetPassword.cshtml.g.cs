#pragma checksum "D:\Course-samples\Api\Sample\AccountManager - MainVersion\AccountManager\Views\ResetPassword\ResetPassword.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "65255a05fab5abae999b31169455b68a898ee12f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ResetPassword_ResetPassword), @"mvc.1.0.view", @"/Views/ResetPassword/ResetPassword.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"65255a05fab5abae999b31169455b68a898ee12f", @"/Views/ResetPassword/ResetPassword.cshtml")]
    public class Views_ResetPassword_ResetPassword : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ViewModels.Account.ForgetPasswordWithEmailConfirmCode>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div style=\"text-align: center;padding:10px;border:1px solid gray; box-shadow: 0px 10px 5px gray;border-radius: 10px;\">\r\n    <h1>ایمیل بازیابی رمز عبور</h1>\r\n    <hr>\r\n    <p>\r\n        برای بازیابی رمز عبور خود ، روی لینک زیر کلیک کنید\r\n    </p>\r\n    <a");
            BeginWriteAttribute("href", " href=", 317, "", 341, 1);
#nullable restore
#line 9 "D:\Course-samples\Api\Sample\AccountManager - MainVersion\AccountManager\Views\ResetPassword\ResetPassword.cshtml"
WriteAttributeValue("", 323, Model.CallBackUrl, 323, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("\r\n       style=\"font-size: 24px;text-decoration: none;color:green;text-shadow: 5px 5px 10px gray;\">\r\n        بازیابی رمز عبور\r\n    </a>\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ViewModels.Account.ForgetPasswordWithEmailConfirmCode> Html { get; private set; }
    }
}
#pragma warning restore 1591