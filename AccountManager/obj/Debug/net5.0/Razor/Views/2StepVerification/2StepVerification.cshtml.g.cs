#pragma checksum "D:\Course-samples\Api\Sample\AccountManager - MainVersion\AccountManager\Views\2StepVerification\2StepVerification.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c1597df2edcae6e0b8882de251738b830b85f101"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_2StepVerification_2StepVerification), @"mvc.1.0.view", @"/Views/2StepVerification/2StepVerification.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c1597df2edcae6e0b8882de251738b830b85f101", @"/Views/2StepVerification/2StepVerification.cshtml")]
    public class Views_2StepVerification_2StepVerification : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ViewModels.Account.TwoStepVerificationEmailDto>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div style=\"text-align: center;padding:10px;border:1px solid gray; box-shadow: 0px 10px 5px gray;border-radius: 10px;\">\r\n    <h1>ایمیل جهت اعتبارسنجی شما</h1>\r\n    <hr>\r\n    <p>\r\n        ");
#nullable restore
#line 7 "D:\Course-samples\Api\Sample\AccountManager - MainVersion\AccountManager\Views\2StepVerification\2StepVerification.cshtml"
   Write(Model.Token);

#line default
#line hidden
#nullable disable
            WriteLiteral(" : کد اعتبار سنجی شما\r\n    </p>\r\n\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ViewModels.Account.TwoStepVerificationEmailDto> Html { get; private set; }
    }
}
#pragma warning restore 1591