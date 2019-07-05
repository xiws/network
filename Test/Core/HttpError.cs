using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core
{
    class HttpError:Exception
    {
        public float Code { get; set; }
        public HttpError(float code, string msg) : base(msg)
        {
            Code = code;
            
        }
        public HttpError(float code) : base(code.ToString())
        {
            string msg= Dic[code];
        }
        static Dictionary<float, string> Dic = new Dictionary<float, string>()
        {
            /*{ 400,"HTTP 400 - 请求无效" },
            { 401.1F,"HTTP 401.1 - 未授权：登录失败" },
            {401.2f, "HTTP 401.2 - 未授权：服务器配置问题导致登录失败"},
            {401.3f,"HTTP 401.3 - ACL 禁止访问资源"},
            {401.4f,"HTTP 401.4 - 未授权：授权被筛选器拒绝"},
            {401.5f,"HTTP 401.5 - 未授权：ISAPI 或 CGI 授权失败"},
            {403,"HTTP 403 - 禁止访问"},
            {403.2f,"HTTP 403 - 对 Internet 服务管理器 的访问仅限于 Localhost"},
            {403.1f,"HTTP 403.1 禁止访问：禁止可执行访问"},
            {403.2f,"HTTP 403.2 - 禁止访问：禁止读访问"},
            {403.3f,"HTTP 403.3 - 禁止访问：禁止写访问"},
            {403.4f,"HTTP 403.4 - 禁止访问：要求 SSL"},
            {403.5f,"HTTP 403.5 - 禁止访问：要求 SSL 128"},
            {403.6f,"HTTP 403.6 - 禁止访问：IP 地址被拒绝"},
            {403.7f,"HTTP 403.7 - 禁止访问：要求客户证书"},
            {403.8f,"HTTP 403.8 - 禁止访问：禁止站点访问"},
            {403.9f,"HTTP 403.9 - 禁止访问：连接的用户过多"},
            {403.10f,"HTTP 403.10 - 禁止访问：配置无效"},
            {403.11f,"HTTP 403.11 - 禁止访问：密码更改"},
            {403.12f,"HTTP 403.12 - 禁止访问：映射器拒绝访问"},
            {403.13f,"HTTP 403.13 - 禁止访问：客户证书已被吊销"},
            {403.15f,"HTTP 403.15 - 禁止访问：客户访问许可过多"},
            {403.16f,"HTTP 403.16 - 禁止访问：客户证书不可信或者无效"},
            {403.17f,"HTTP 403.17 - 禁止访问：客户证书已经到期或者尚未生效 HTTP 404.1 "},
            {404,"HTTP 404- 无法找到文件"},
            {405,"HTTP 405 - 资源被禁止"},
            {406,"HTTP 406 - 无法接受"},
            {407,"HTTP 407 - 要求代理身份验证"},
            {410,"HTTP 410 - 永远不可用"},
            {412,"HTTP 412 - 先决条件失败"},
            {414,"HTTP 414 - 请求 - URI 太长"},
            {500f,"HTTP 500 - 内部服务器错误"},
            {500.100f,"HTTP 500.100 - 内部服务器错误 - ASP 错误"},
            {500.11f,"HTTP 500-11 服务器关闭"},
            {500.12f,"HTTP 500-12 应用程序重新启动"},
            {500.13f,"HTTP 500-13 - 服务器太忙"},
            {500.14f,"HTTP 500-14 - 应用程序无效"},
            {500.15f,"HTTP 500-15 - 不允许请求 global.asa"},
            {501f,"Error 501 - 未实现"},
            {502f,"HTTP 502 - 网关错误" }*/
                 {404,"HTTP 404- 无法找到文件"}
        };
    }
}
