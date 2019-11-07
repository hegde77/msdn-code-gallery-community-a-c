# Building an Office Web Apps (OWA) WOPI Host
## Requires
- Visual Studio 2013
## License
- MS-LPL
## Technologies
- Office Web Apps
- ASP.NET MVC 4
- ASP.NET Web API
- SharePoint Server 2013
- Office Development
- ASP.NET MVC 5
## Topics
- Office Web Apps
- WOPI
## Updated
- 10/15/2015
## Description

<p><strong>UPDATE: 2015-10-15</strong></p>
<p><em>There is another &int; published here:&nbsp;<a title="https://github.com/CedarLogic/Office-Online-Test-Tools-and-Documentation/tree/master/samples" href="https://github.com/CedarLogic/Office-Online-Test-Tools-and-Documentation/tree/master/samples" target="_blank">https://github.com/CedarLogic/Office-Online-Test-Tools-and-Documentation/tree/master/samples</a></em></p>
<p>Additionally, for Storage Partners - you can view more here:&nbsp;<a href="http://dev.office.com/programs/officecloudstorage" target="_blank">http://dev.office.com/programs/officecloudstorage</a></p>
<p>And <strong>Documentation</strong> - here's a bunch more:&nbsp;<a href="https://wopi.readthedocs.org/en/latest/intro.html" target="_blank">https://wopi.readthedocs.org/en/latest/intro.html</a></p>
<p><strong>UPDATE: 2015-July-31</strong></p>
<p><em>The solution and project have been updated to MVC5, and Web API 2.&nbsp; In addition, editing PowerPoint (PPTX), and Excel files has been added.&nbsp; Word Editing is not part of the solution. Also, PDF viewing is enabled.</em></p>
<p>&nbsp;</p>
<p>The latest version of OWA in an on-premises deployment decouples the dependency on SharePoint. For those organizations that perhaps have invested somewhat in non-SharePoint content or document management systems, this offers an opportunity to provide the
 OWA experience with content from your site.</p>
<h3><span style="font-size:1.5em">WOPI Host</span></h3>
<p>The WOPI host protocol is defined at this location: <a href="http://msdn.microsoft.com/en-us/library/hh643135(v=office.12).aspx">
http://msdn.microsoft.com/en-us/library/hh643135(v=office.12).aspx</a></p>
<p>There&rsquo;s a great overview, introducing WOPI in a blog post from the Office development team here:
<a href="http://blogs.msdn.com/b/officedevdocs/archive/2013/03/21/introducing-wopi.aspx">
http://blogs.msdn.com/b/officedevdocs/archive/2013/03/21/introducing-wopi.aspx</a></p>
<p>In addition, a good overview of the architecture in 2013 (vs. 2010) is shown here:</p>
<p><a href="http://technet.microsoft.com/en-us/library/jj219437.aspx">http://technet.microsoft.com/en-us/library/jj219437.aspx</a></p>
<h2>Callback interface</h2>
<p>Note that a WOPI host has to respond to a direct call from OWA for the content. That is illustrated in the above post with this sequence diagram:</p>
<p><img id="92720" src="92720-7128.intro_5f00_wopi_5f00_5c7408b7%5b1%5d.jpg" alt="" width="582" height="457"></p>
<p>&nbsp;</p>
<h2>Building WOPI Host</h2>
<p>So, for this post, we&rsquo;re going to cover a working WOPI host that will utilize OWA for display content (Word, Excel, and PowerPoint) with an OWA on-premises deployment.</p>
<h2>Primary Interfaces</h2>
<p>To get started, the bare minimum implementation, for viewing, requires 2 interfaces implemented as REST endpoints on your WOPI Host.</p>
<p>The solution contains a series of API controllers. The FilesController implements the 2 prmiamry interfaces &ndash; the first is a GET which returns the file information; the second returns the content as a stream.</p>
<h4>Files</h4>
<table border="1" cellpadding="0" width="590">
<tbody>
<tr>
<td width="280">
<p><strong>API</strong></p>
</td>
<td width="308">
<p><strong>Description</strong></p>
</td>
</tr>
<tr>
<td width="280">
<p><a href="http://localhost:55574/Help/Api/GET-api-wopi-files-name_access_token">GET api/wopi/files/{name}?access_token={access_token}</a></p>
</td>
<td width="308">
<p>Required for WOPI interface - on initial view</p>
</td>
</tr>
<tr>
<td width="280">
<p><a href="http://localhost:55574/Help/Api/GET-api-wopi-files-name-contents_access_token">GET api/wopi/files/{name}/contents?access_token={access_token}</a></p>
</td>
<td width="308">
<p>Required for View WOPI interface - returns stream of document.</p>
</td>
</tr>
</tbody>
</table>
<p>&nbsp;</p>
<h2>Discovery XML</h2>
<p>Within the ~/App_Data location, there&rsquo;s a discovery.xml file. This is retrieved using the following URL from the OWA server. That XML just needs to be saved to the location.</p>
<p><a href="http://owa1.wingtip.com/hosting/discovery">http://owa1.wingtip.com/hosting/discovery</a></p>
<p>The solution builds the proper full URL based upon the file type, by examining this file.</p>
<h2>Uploading Files / Link Generation</h2>
<p>For the sake of testing, you can upload files using the Upload API. This will accept multiple files and return a JSON result that is a collection of Links, with access tokens for each file.</p>
<p>The Link generation is used to generate a fully qualified link that can be used to view an Office file on OWA which will be consumed from the WOPI host.</p>
<h2>Access Token</h2>
<p>OWA supports the WOPI host use of an access token. Note that the sample provides a HMACSHA256 of the file name using a random generated salt value.</p>
<h2>Deployment</h2>
<p>Note that the WOPI host MUST be HTTP addressable from the OWA server. In this sample, you also have to turn off HTTPS. Check the OWA TechNet articles on how.</p>
<h2>Source Code:</h2>
<p>The solution file is here&hellip;</p>
<h3>Here&rsquo;s the GetInfo portion</h3>
<div class="wlWriterEditableSmartContent" id="scid:f32c3428-b7e9-4f15-a8ea-c502c7ff2e88:5b44723c-c77c-435e-a385-141fa1fb4ac9">
<pre class="brush: x_x_x_x_x_c#;"><div class="scriptcode"><div class="pluginEditHolder" pluginCommand="mceScriptCode"><div class="title"><span>C#</span></div><div class="pluginLinkHolder"><span class="pluginEditHolderLink">Edit</span>|<span class="pluginRemoveHolderLink">Remove</span></div><span class="hidden">csharp</span><pre class="hidden">        /// &lt;summary&gt;
        /// Required for WOPI interface - on initial view
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;name&quot;&gt;file name&lt;/param&gt;
        /// &lt;param name=&quot;access_token&quot;&gt;token that WOPI server will know&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        public CheckFileInfo Get(string name, string access_token)
        {
            Validate(name, access_token);

            var fileInfo = _fileHelper.GetFileInfo(name);
            return fileInfo;
        }</pre>
<div class="preview">
<pre class="js">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">///&nbsp;&lt;summary&gt;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">///&nbsp;Required&nbsp;for&nbsp;WOPI&nbsp;interface&nbsp;-&nbsp;on&nbsp;initial&nbsp;view</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">///&nbsp;&lt;/summary&gt;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">///&nbsp;&lt;param&nbsp;name=&quot;name&quot;&gt;file&nbsp;name&lt;/param&gt;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">///&nbsp;&lt;param&nbsp;name=&quot;access_token&quot;&gt;token&nbsp;that&nbsp;WOPI&nbsp;server&nbsp;will&nbsp;know&lt;/param&gt;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">///&nbsp;&lt;returns&gt;&lt;/returns&gt;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;public&nbsp;CheckFileInfo&nbsp;Get(string&nbsp;name,&nbsp;string&nbsp;access_token)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Validate(name,&nbsp;access_token);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;fileInfo&nbsp;=&nbsp;_fileHelper.GetFileInfo(name);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;fileInfo;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span></pre>
</div>
</div>
</div>
<div class="endscriptcode">&nbsp;</div>
</pre>
</div>
<p>&nbsp;</p>
<h3>And the Contents portion</h3>
<div class="wlWriterEditableSmartContent" id="scid:f32c3428-b7e9-4f15-a8ea-c502c7ff2e88:0c92ed0e-d027-42fa-801a-830274bddc70">
<pre class="brush: x_x_x_x_x_c#;"><div class="scriptcode"><div class="pluginEditHolder" pluginCommand="mceScriptCode"><div class="title"><span>C#</span></div><div class="pluginLinkHolder"><span class="pluginEditHolderLink">Edit</span>|<span class="pluginRemoveHolderLink">Remove</span></div><span class="hidden">csharp</span><pre class="hidden"> /// &lt;summary&gt;
 /// Required for View WOPI interface - returns stream of document.
 /// &lt;/summary&gt;
 /// &lt;param name=&quot;name&quot;&gt;file name&lt;/param&gt;
 /// &lt;param name=&quot;access_token&quot;&gt;token that WOPI server will know&lt;/param&gt;
 /// &lt;returns&gt;&lt;/returns&gt;
 public HttpResponseMessage GetFile(string name, string access_token)
 {
     try
     {
         Validate(name, access_token);
         var file = HostingEnvironment.MapPath(&quot;~/App_Data/&quot; &#43; name);
         var rv = new HttpResponseMessage(HttpStatusCode.OK);
         var stream = new FileStream(file, FileMode.Open, FileAccess.Read);

         rv.Content = new StreamContent(stream);
         rv.Content.Headers.ContentType = new MediaTypeHeaderValue(&quot;application/octet-stream&quot;);
         return rv;

     }
     catch (Exception ex)
     {
         var rv = new HttpResponseMessage(HttpStatusCode.InternalServerError);
         var stream = new MemoryStream(UTF8Encoding.Default.GetBytes(ex.Message ?? &quot;&quot;));
         rv.Content = new StreamContent(stream);
         return rv;
     }
 }</pre>
<div class="preview">
<pre class="js">&nbsp;<span class="js__sl_comment">///&nbsp;&lt;summary&gt;</span>&nbsp;
&nbsp;<span class="js__sl_comment">///&nbsp;Required&nbsp;for&nbsp;View&nbsp;WOPI&nbsp;interface&nbsp;-&nbsp;returns&nbsp;stream&nbsp;of&nbsp;document.</span>&nbsp;
&nbsp;<span class="js__sl_comment">///&nbsp;&lt;/summary&gt;</span>&nbsp;
&nbsp;<span class="js__sl_comment">///&nbsp;&lt;param&nbsp;name=&quot;name&quot;&gt;file&nbsp;name&lt;/param&gt;</span>&nbsp;
&nbsp;<span class="js__sl_comment">///&nbsp;&lt;param&nbsp;name=&quot;access_token&quot;&gt;token&nbsp;that&nbsp;WOPI&nbsp;server&nbsp;will&nbsp;know&lt;/param&gt;</span>&nbsp;
&nbsp;<span class="js__sl_comment">///&nbsp;&lt;returns&gt;&lt;/returns&gt;</span>&nbsp;
&nbsp;public&nbsp;HttpResponseMessage&nbsp;GetFile(string&nbsp;name,&nbsp;string&nbsp;access_token)&nbsp;
&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">try</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Validate(name,&nbsp;access_token);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;file&nbsp;=&nbsp;HostingEnvironment.MapPath(<span class="js__string">&quot;~/App_Data/&quot;</span>&nbsp;&#43;&nbsp;name);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;rv&nbsp;=&nbsp;<span class="js__operator">new</span>&nbsp;HttpResponseMessage(HttpStatusCode.OK);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;stream&nbsp;=&nbsp;<span class="js__operator">new</span>&nbsp;FileStream(file,&nbsp;FileMode.Open,&nbsp;FileAccess.Read);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;rv.Content&nbsp;=&nbsp;<span class="js__operator">new</span>&nbsp;StreamContent(stream);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;rv.Content.Headers.ContentType&nbsp;=&nbsp;<span class="js__operator">new</span>&nbsp;MediaTypeHeaderValue(<span class="js__string">&quot;application/octet-stream&quot;</span>);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;rv;&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">catch</span>&nbsp;(Exception&nbsp;ex)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;rv&nbsp;=&nbsp;<span class="js__operator">new</span>&nbsp;HttpResponseMessage(HttpStatusCode.InternalServerError);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;stream&nbsp;=&nbsp;<span class="js__operator">new</span>&nbsp;MemoryStream(UTF8Encoding.Default.GetBytes(ex.Message&nbsp;??&nbsp;<span class="js__string">&quot;&quot;</span>));&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;rv.Content&nbsp;=&nbsp;<span class="js__operator">new</span>&nbsp;StreamContent(stream);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;rv;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;<span class="js__brace">}</span></pre>
</div>
</div>
</div>
<div class="endscriptcode">&nbsp;</div>
</pre>
</div>
<p>&nbsp;</p>
<h3>And, KeyGen &ndash; which generates the hash values</h3>
<div class="wlWriterEditableSmartContent" id="scid:f32c3428-b7e9-4f15-a8ea-c502c7ff2e88:e5239774-895d-47a8-9534-2b6f8644502c">
<pre class="brush: x_x_x_x_x_c#;"><div class="scriptcode"><div class="pluginEditHolder" pluginCommand="mceScriptCode"><div class="title"><span>C#</span></div><div class="pluginLinkHolder"><span class="pluginEditHolderLink">Edit</span>|<span class="pluginRemoveHolderLink">Remove</span></div><span class="hidden">csharp</span><pre class="hidden">namespace MainWeb.Helpers
{
    public interface IKeyGen
    {
        string GetHash(string value);

        bool Validate(string name, string access_token);
    }
    public class KeyGen : IKeyGen
    {
        byte[] _key;
        int _saltLength = 8;

        static RNGCryptoServiceProvider s_rngCsp = new RNGCryptoServiceProvider();

        public KeyGen()
        {
            var key = WebConfigurationManager.AppSettings[&quot;appHmacKey&quot;];
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(&quot;must supply a HmacKey - check config&quot;);
            _key = Encoding.UTF8.GetBytes(key);
        }

        public string GetHash(string value)
        {
            byte[] salt = new byte[_saltLength];
            s_rngCsp.GetBytes(salt);

            var saltStr = Convert.ToBase64String(salt);
            return GetHash(value, saltStr);
        }

        private string GetHash(string value, string saltStr)
        {
            //Not really secure; must use random salt to ensure non-repeat....
            HMACSHA256 hmac = new HMACSHA256(_key);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(saltStr &#43; value));
            var rv = Convert.ToBase64String(hash);
            return saltStr &#43; rv;
        }


        public bool Validate(string name, string access_token)
        {
            var targetHash = GetHash(name, access_token.Substring(0,_saltLength &#43; 4));  //hack for base64 form
            return String.Equals(access_token, targetHash);
        }


    }
}
 </pre>
<div class="preview">
<pre class="js">namespace&nbsp;MainWeb.Helpers&nbsp;
<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;public&nbsp;interface&nbsp;IKeyGen&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;string&nbsp;GetHash(string&nbsp;value);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;bool&nbsp;Validate(string&nbsp;name,&nbsp;string&nbsp;access_token);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;public&nbsp;class&nbsp;KeyGen&nbsp;:&nbsp;IKeyGen&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;byte[]&nbsp;_key;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;int&nbsp;_saltLength&nbsp;=&nbsp;<span class="js__num">8</span>;&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;static&nbsp;RNGCryptoServiceProvider&nbsp;s_rngCsp&nbsp;=&nbsp;<span class="js__operator">new</span>&nbsp;RNGCryptoServiceProvider();&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;public&nbsp;KeyGen()&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;key&nbsp;=&nbsp;WebConfigurationManager.AppSettings[<span class="js__string">&quot;appHmacKey&quot;</span>];&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">if</span>&nbsp;(string.IsNullOrEmpty(key))&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">throw</span>&nbsp;<span class="js__operator">new</span>&nbsp;ArgumentNullException(<span class="js__string">&quot;must&nbsp;supply&nbsp;a&nbsp;HmacKey&nbsp;-&nbsp;check&nbsp;config&quot;</span>);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;_key&nbsp;=&nbsp;Encoding.UTF8.GetBytes(key);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;public&nbsp;string&nbsp;GetHash(string&nbsp;value)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;byte[]&nbsp;salt&nbsp;=&nbsp;<span class="js__operator">new</span>&nbsp;byte[_saltLength];&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;s_rngCsp.GetBytes(salt);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;saltStr&nbsp;=&nbsp;Convert.ToBase64String(salt);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;GetHash(value,&nbsp;saltStr);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;private&nbsp;string&nbsp;GetHash(string&nbsp;value,&nbsp;string&nbsp;saltStr)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//Not&nbsp;really&nbsp;secure;&nbsp;must&nbsp;use&nbsp;random&nbsp;salt&nbsp;to&nbsp;ensure&nbsp;non-repeat....</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;HMACSHA256&nbsp;hmac&nbsp;=&nbsp;<span class="js__operator">new</span>&nbsp;HMACSHA256(_key);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;hash&nbsp;=&nbsp;hmac.ComputeHash(Encoding.UTF8.GetBytes(saltStr&nbsp;&#43;&nbsp;value));&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;rv&nbsp;=&nbsp;Convert.ToBase64String(hash);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;saltStr&nbsp;&#43;&nbsp;rv;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;public&nbsp;bool&nbsp;Validate(string&nbsp;name,&nbsp;string&nbsp;access_token)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;targetHash&nbsp;=&nbsp;GetHash(name,&nbsp;access_token.Substring(<span class="js__num">0</span>,_saltLength&nbsp;&#43;&nbsp;<span class="js__num">4</span>));&nbsp;&nbsp;<span class="js__sl_comment">//hack&nbsp;for&nbsp;base64&nbsp;form</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;<span class="js__object">String</span>.Equals(access_token,&nbsp;targetHash);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
<span class="js__brace">}</span>&nbsp;
&nbsp;</pre>
</div>
</div>
</div>
<div class="endscriptcode">&nbsp;</div>
<br></pre>
</div>
<h3>File Helper</h3>
<div class="wlWriterEditableSmartContent" id="scid:f32c3428-b7e9-4f15-a8ea-c502c7ff2e88:f4f26537-a891-4750-bbbf-614d37cc030f">
<pre class="brush: x_x_x_x_x_c#;"><div class="scriptcode"><div class="pluginEditHolder" pluginCommand="mceScriptCode"><div class="title"><span>C#</span></div><div class="pluginLinkHolder"><span class="pluginEditHolderLink">Edit</span>|<span class="pluginRemoveHolderLink">Remove</span></div><span class="hidden">csharp</span><pre class="hidden">public interface IFileHelper
{
    CheckFileInfo GetFileInfo(string name);
}

public class FileHelper : IFileHelper
{
    public CheckFileInfo GetFileInfo(string name)
    {
        var fileName = GetFileName(name);
        FileInfo info = new FileInfo(fileName);
        string sha256 = &quot;&quot;;

        using (FileStream stream = File.OpenRead(fileName))
        using (var sha = SHA256.Create())
        {
            byte[] checksum = sha.ComputeHash(stream);
            sha256 = Convert.ToBase64String(checksum);
        }

        var rv = new CheckFileInfo
        {
            BaseFileName = info.Name,
            OwnerId = &quot;admin&quot;,
            Size = info.Length,
            SHA256 = sha256,
            Version = info.LastWriteTimeUtc.ToString(&quot;s&quot;)
        };

        return rv;
    }


    internal string GetFileName(string name)
    {
        var file = HostingEnvironment.MapPath(&quot;~/App_Data/&quot; &#43; name);
        return file;
    }
}</pre>
<div class="preview">
<pre class="js">public&nbsp;interface&nbsp;IFileHelper&nbsp;
<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;CheckFileInfo&nbsp;GetFileInfo(string&nbsp;name);&nbsp;
<span class="js__brace">}</span>&nbsp;
&nbsp;
public&nbsp;class&nbsp;FileHelper&nbsp;:&nbsp;IFileHelper&nbsp;
<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;public&nbsp;CheckFileInfo&nbsp;GetFileInfo(string&nbsp;name)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;fileName&nbsp;=&nbsp;GetFileName(name);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;FileInfo&nbsp;info&nbsp;=&nbsp;<span class="js__operator">new</span>&nbsp;FileInfo(fileName);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;string&nbsp;sha256&nbsp;=&nbsp;<span class="js__string">&quot;&quot;</span>;&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;using&nbsp;(FileStream&nbsp;stream&nbsp;=&nbsp;File.OpenRead(fileName))&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;using&nbsp;(<span class="js__statement">var</span>&nbsp;sha&nbsp;=&nbsp;SHA256.Create())&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;byte[]&nbsp;checksum&nbsp;=&nbsp;sha.ComputeHash(stream);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;sha256&nbsp;=&nbsp;Convert.ToBase64String(checksum);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;rv&nbsp;=&nbsp;<span class="js__operator">new</span>&nbsp;CheckFileInfo&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;BaseFileName&nbsp;=&nbsp;info.Name,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;OwnerId&nbsp;=&nbsp;<span class="js__string">&quot;admin&quot;</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Size&nbsp;=&nbsp;info.Length,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SHA256&nbsp;=&nbsp;sha256,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Version&nbsp;=&nbsp;info.LastWriteTimeUtc.ToString(<span class="js__string">&quot;s&quot;</span>)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>;&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;rv;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;internal&nbsp;string&nbsp;GetFileName(string&nbsp;name)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;file&nbsp;=&nbsp;HostingEnvironment.MapPath(<span class="js__string">&quot;~/App_Data/&quot;</span>&nbsp;&#43;&nbsp;name);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;file;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
<span class="js__brace">}</span></pre>
</div>
</div>
</div>
<div class="endscriptcode">&nbsp;</div>
</pre>
</div>
<h3>Finally, for this Post a WOPI helper class</h3>
<div class="wlWriterEditableSmartContent" id="scid:f32c3428-b7e9-4f15-a8ea-c502c7ff2e88:afbcb1db-327b-4f82-98ca-2c0fdcfe837b">
<pre class="brush: x_x_x_x_x_c#;"><div class="scriptcode"><div class="pluginEditHolder" pluginCommand="mceScriptCode"><div class="title"><span>C#</span></div><div class="pluginLinkHolder"><span class="pluginEditHolderLink">Edit</span>|<span class="pluginRemoveHolderLink">Remove</span></div><span class="hidden">csharp</span><pre class="hidden">public class WopiAppHelper
{
    string _discoveryFile;
    WopiHost.wopidiscovery _wopiDiscovery;

    public WopiAppHelper(string discoveryXml)
    {
        _discoveryFile = discoveryXml;

        using (StreamReader file = new StreamReader(discoveryXml))
        {
            XmlSerializer reader = new XmlSerializer(typeof(WopiHost.wopidiscovery));
            var wopiDiscovery = reader.Deserialize(file) as WopiHost.wopidiscovery;
            _wopiDiscovery = wopiDiscovery;
        }
    }


    public WopiHost.wopidiscoveryNetzoneApp GetZone(string AppName)
    {
        var rv = _wopiDiscovery.netzone.app.Where(c =&gt; c.name == AppName).FirstOrDefault();
        return rv;
    }

    public string  GetDocumentLink(string wopiHostandFile)
    {
        var fileName = wopiHostandFile.Substring(wopiHostandFile.LastIndexOf('/') &#43; 1);
        var accessToken = GetToken(fileName);
        var fileExt = fileName.Substring(fileName.LastIndexOf('.') &#43; 1);
        var tt = _wopiDiscovery.netzone.app.AsEnumerable().Where(c =&gt; c.action.Where(d =&gt; d.ext == fileExt).Count() &gt; 0);

        var appName = tt.FirstOrDefault();

        if (null == appName) throw new ArgumentException(&quot;invalid file extension &quot; &#43; fileExt);

        var rv = GetDocumentLink(appName.name, fileExt, wopiHostandFile, accessToken);

        return rv;
    }

    string GetToken(string fileName)
    {
        KeyGen keyGen = new KeyGen();
        var rv = keyGen.GetHash(fileName);

        return HttpUtility.UrlEncode(rv);
    }

    const string s_WopiHostFormat = &quot;{0}?WOPISrc={1}&amp;access_token={2}&quot;;
    public string GetDocumentLink(string appName, string fileExtension, string wopiHostAndFile, string accessToken)
    {
        var wopiHostUrlsafe = HttpUtility.UrlEncode(wopiHostAndFile.Replace(&quot; &quot;, &quot;%20&quot;));
        var appStuff = _wopiDiscovery.netzone.app.Where(c =&gt; c.name == appName).FirstOrDefault();

        if (null == appStuff)
            throw new ApplicationException(&quot;Can't locate App: &quot; &#43; appName);


        var appAction = appStuff.action.Where(c =&gt; c.ext == fileExtension).FirstOrDefault();
                    if (null == appAction)
            throw new ApplicationException(&quot;Can't locate UrlSrc for : &quot; &#43; appName);

        var endPoint = appAction.urlsrc.IndexOf('?');
        var fullPath = string.Format(s_WopiHostFormat, appAction.urlsrc.Substring(0, endPoint), wopiHostUrlsafe, accessToken); 

        return fullPath;
    }
}
</pre>
<div class="preview">
<pre class="js">public&nbsp;class&nbsp;WopiAppHelper&nbsp;
<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;string&nbsp;_discoveryFile;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;WopiHost.wopidiscovery&nbsp;_wopiDiscovery;&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;public&nbsp;WopiAppHelper(string&nbsp;discoveryXml)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;_discoveryFile&nbsp;=&nbsp;discoveryXml;&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;using&nbsp;(StreamReader&nbsp;file&nbsp;=&nbsp;<span class="js__operator">new</span>&nbsp;StreamReader(discoveryXml))&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;XmlSerializer&nbsp;reader&nbsp;=&nbsp;<span class="js__operator">new</span>&nbsp;XmlSerializer(<span class="js__operator">typeof</span>(WopiHost.wopidiscovery));&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;wopiDiscovery&nbsp;=&nbsp;reader.Deserialize(file)&nbsp;as&nbsp;WopiHost.wopidiscovery;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;_wopiDiscovery&nbsp;=&nbsp;wopiDiscovery;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;public&nbsp;WopiHost.wopidiscoveryNetzoneApp&nbsp;GetZone(string&nbsp;AppName)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;rv&nbsp;=&nbsp;_wopiDiscovery.netzone.app.Where(c&nbsp;=&gt;&nbsp;c.name&nbsp;==&nbsp;AppName).FirstOrDefault();&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;rv;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;public&nbsp;string&nbsp;&nbsp;GetDocumentLink(string&nbsp;wopiHostandFile)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;fileName&nbsp;=&nbsp;wopiHostandFile.Substring(wopiHostandFile.LastIndexOf(<span class="js__string">'/'</span>)&nbsp;&#43;&nbsp;<span class="js__num">1</span>);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;accessToken&nbsp;=&nbsp;GetToken(fileName);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;fileExt&nbsp;=&nbsp;fileName.Substring(fileName.LastIndexOf(<span class="js__string">'.'</span>)&nbsp;&#43;&nbsp;<span class="js__num">1</span>);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;tt&nbsp;=&nbsp;_wopiDiscovery.netzone.app.AsEnumerable().Where(c&nbsp;=&gt;&nbsp;c.action.Where(d&nbsp;=&gt;&nbsp;d.ext&nbsp;==&nbsp;fileExt).Count()&nbsp;&gt;&nbsp;<span class="js__num">0</span>);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;appName&nbsp;=&nbsp;tt.FirstOrDefault();&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">if</span>&nbsp;(null&nbsp;==&nbsp;appName)&nbsp;<span class="js__statement">throw</span>&nbsp;<span class="js__operator">new</span>&nbsp;ArgumentException(<span class="js__string">&quot;invalid&nbsp;file&nbsp;extension&nbsp;&quot;</span>&nbsp;&#43;&nbsp;fileExt);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;rv&nbsp;=&nbsp;GetDocumentLink(appName.name,&nbsp;fileExt,&nbsp;wopiHostandFile,&nbsp;accessToken);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;rv;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;string&nbsp;GetToken(string&nbsp;fileName)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;KeyGen&nbsp;keyGen&nbsp;=&nbsp;<span class="js__operator">new</span>&nbsp;KeyGen();&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;rv&nbsp;=&nbsp;keyGen.GetHash(fileName);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;HttpUtility.UrlEncode(rv);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">const</span>&nbsp;string&nbsp;s_WopiHostFormat&nbsp;=&nbsp;<span class="js__string">&quot;{0}?WOPISrc={1}&amp;access_token={2}&quot;</span>;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;public&nbsp;string&nbsp;GetDocumentLink(string&nbsp;appName,&nbsp;string&nbsp;fileExtension,&nbsp;string&nbsp;wopiHostAndFile,&nbsp;string&nbsp;accessToken)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;wopiHostUrlsafe&nbsp;=&nbsp;HttpUtility.UrlEncode(wopiHostAndFile.Replace(<span class="js__string">&quot;&nbsp;&quot;</span>,&nbsp;<span class="js__string">&quot;%20&quot;</span>));&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;appStuff&nbsp;=&nbsp;_wopiDiscovery.netzone.app.Where(c&nbsp;=&gt;&nbsp;c.name&nbsp;==&nbsp;appName).FirstOrDefault();&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">if</span>&nbsp;(null&nbsp;==&nbsp;appStuff)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">throw</span>&nbsp;<span class="js__operator">new</span>&nbsp;ApplicationException(<span class="js__string">&quot;Can't&nbsp;locate&nbsp;App:&nbsp;&quot;</span>&nbsp;&#43;&nbsp;appName);&nbsp;
&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;appAction&nbsp;=&nbsp;appStuff.action.Where(c&nbsp;=&gt;&nbsp;c.ext&nbsp;==&nbsp;fileExtension).FirstOrDefault();&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">if</span>&nbsp;(null&nbsp;==&nbsp;appAction)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">throw</span>&nbsp;<span class="js__operator">new</span>&nbsp;ApplicationException(<span class="js__string">&quot;Can't&nbsp;locate&nbsp;UrlSrc&nbsp;for&nbsp;:&nbsp;&quot;</span>&nbsp;&#43;&nbsp;appName);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;endPoint&nbsp;=&nbsp;appAction.urlsrc.IndexOf(<span class="js__string">'?'</span>);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;fullPath&nbsp;=&nbsp;string.Format(s_WopiHostFormat,&nbsp;appAction.urlsrc.Substring(<span class="js__num">0</span>,&nbsp;endPoint),&nbsp;wopiHostUrlsafe,&nbsp;accessToken);&nbsp;&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;fullPath;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
<span class="js__brace">}</span>&nbsp;
</pre>
</div>
</div>
</div>
<div class="endscriptcode">&nbsp;</div>
<br></pre>
</div>