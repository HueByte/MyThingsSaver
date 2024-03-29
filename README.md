<p align="center">
   <a href="https://github.com/HueByte/MyThingsSaver">
      <img src="https://raw.githubusercontent.com/HueByte/MyThingsSaver/master/backend/App/client/public/favicon.png" title="MyThingsSaver Logo"/>
   </a>
</p>

# Disclaimer
Currently working on a new release (v2)<br>
Things left to finish:
- Public entries
- Admin user management
- Quality of Life updates

# `👾 My Things Saver`
Introducing a sleek and user-friendly app with a modern design, perfect for saving all your important information in an organized and visually appealing manner.<br /> With a robust Markdown editor, you can effortlessly jot down notes, create lists, manage tasks and share it with others!
</br>

<img style="border-radius: 15px" src="https://github.com/HueByte/MyThingsSaver/blob/dev/git/assets/homepage.png" />
<img style="border-radius: 15px" src="https://github.com/HueByte/MyThingsSaver/blob/dev/git/assets/entry.png" />

# `🌀 Start`

### System

| OS | App Version | 
| --- | --- | 
| Linux 64bit | Linux64_Standalone |
| Linux-arm 32bit | Linux86-arm |
| MacOS 64bit | MacOSx64 |
| Windows 64bit | Windowsx64 | 
| Windows 32bit | Windowsx86 |

> If you have problem running the app try installing [ASP.NET core runtime 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) for your OS

### `Windows` (Link outdated as for now)
1. Download [MyThingsSaver.zip](https://github.com/HueByte/MyThingsSaver/releases/tag/v1.0.0)
2. Unpack it
3. Locate **App.exe** in the root directory
4. Run **App.exe**
5. Type http://localhost/ in your browser

### `Linux` (Link outdated as for now)
1. Download [MyThingsSaver.zip](https://github.com/HueByte/MyThingsSaver/releases/tag/v1.0.0)
2. Unpack it
3. Locate **App.dll** or **App** file in the root directory
4. run command: `dotnet App.dll` / or run `App` file
5. Type http://localhost/ in your browser

### `MacOS` (Link outdated as for now)
1. Download [MyThingsSaver.zip](https://github.com/HueByte/MyThingsSaver/releases/tag/v1.0.0)
2. Unpack it
3. Locate `App` File in the root directory
4. Run `App` file
5. Type http://localhost/ in your browser
</br>

### How to access app
* Type http://localhost/ in your browser (working only on your local machine)
* Type http://YourLocalIP/ in your browser (working only in your local network)
* Type http://YourPublicIP/ in your browser (App needs to be port forwarded - [How to?](https://www.noip.com/support/knowledgebase/general-port-forwarding-guide/))

# `👨‍💻 For Development:` 
### Development requirements
* npm
* .NET 7 SDK

### How to run development?
   1. git clone repository 
   2. go to *MyThingsSaver/src/App*
   3. Move `appsettings.json` from `examples` folder to `src/App` and configure it to your needs
   4. run command: `dotnet run`
</br>

### Configure your development 
- Remember to have self signed developer certificate for local HTTPS
- Copy contents of `.env` files from examples folder (remove `example` part of file name) to `backend/App/client`
- Copy example appsettings.json to `backend/App` or use new generated one from debug folder once you run app once
- Ports are configured in `.env`, `.env.development`, `App.csproj`, `appsettings.json`

### Configure ports
`appsettings.json` file should be inside `backend/App` and `.env` file should be inside `backend/App/client`

1. Set API ports inside `appsettings.json` (`HttpPort`, `HttpsPort`)
2. Set correct Https port to `ASPNETCORE_HTTPS_PORT` variable in `.env`
3. Set client app port to `PORT` variable in `.env`
4. Set correct client port to `<SpaProxyServerUrl>` tag in `App.csproj`

> `HttpsPort` must match `ASPNETCORE_HTTPS_PORT` variable<br>
> `<SpaProxyServerUrl>` must match `PORT` variable

# `⚙️ Configuration `

### Overall Network configuration 
Handled by `Network` object in `appsettings.json`

* Nginx & HTTPS</br>Example nginx configuration file is inside `examples` folder.</br>
   1. Install [nginx](https://www.nginx.com/resources/wiki/start/topics/tutorials/install/)
   2. Replace [nginx configuration file](http://nginx.org/en/docs/beginners_guide.html) with `examples/nginx.conf`
   3. Create `.cert` folder in the nginx root folder
   4. Get SSL certificates from [GreenLock](https://greenlock.domains/) or other provider if you don't have them
   5. Move your certificate files to `.cert` folder
   6. Open and edit `appsettings.json`
   7. Change `Type` in `Network` to `nginx`
   8. If you're using example nginx config you can set `HttpsRedirection` to `false` because it's handled by nginx
   9. If you're using example nginx config you can set `UseHSTS` to `false` because HSTS is handled by nginx
   10. Set ports to different ones than 80 and 443 because nginx will be using them
   11. Set `Issuer` and `Audience` in `JWT` to your domain 
   12. Run nginx
   13. Run App

   If you want to access your application from internet (outside your local network) you need to port forward ports your nginx is configured on, you can visit this guide for that - [How do I port forward?](https://www.noip.com/support/knowledgebase/general-port-forwarding-guide/). You need to forward ports **80** and **443** if you're using example `nginx.conf` file
</br>

### Network
* `Type` determines how app behaviour. If you're using it as standalone version set it to `standalone`, if you're using nginx set it to `nginx` to successfully configure right headers forwarding behaviour
* `HttpsRedirection` set true if you want redirect your users to app on https ports
* `UseHttps` Configures app to run also at `HttpsPort` 
* `UseHSTS` HTTP Strict Transport Security Protocol which enforce using HTTPS and prevents sending any communication over HTTP

#### Disclaimer
> 1. Using settings like `HttpsRedirection`, `UseHSTS` and `UseHttps` might break your app if you don't have SSL certificate and correct proxy server configured (like nginx)
> 2. If you want to run your app locally with https you can set `UseHttps` to true but you have to have trusted localhost SSL certificate 
> 3. Server will be served on your localhost IP by default, that means if your PC has ip for example 192.168.0.100 you can access app by typing http://192.168.0.100/ on your network, or http://localhost/ on your local machine. To check your IP type: `ipconfig` on windows, `ifconfig` on linux or 
</br>

### Logging 
By default logger is set to "Warning" level</br>
You can change it to the following:

| Log Level | Severity | Description |
| --- | --- | --- | 
Trace | 0 | Logs messages only for tracing purposes for the developers.
Debug | 1 |Logs messages for short-term debugging purposes.
Information | 2	| Logs messages for the flow of the application.
Warning | 3 | Logs messages for abnormal or unexpected events in the application flow.
Error |	4 | Logs error messages.
Critical | 	5 | Logs failures messages that require immediate attention.

### AllowedHosts
AllowedHosts is used for host filtering to bind your app to specific hostnames</br>
By default it's `*` which means all hosts are allowed

### ConnectionStrings
Connection strings to your database, by default SQLite with connection contained in `SQLiteConnectionString` is used as your databse</br>
You can change target database in `Database` setting

Default ConnectionString schemes: 
| Name | Scheme | 
| --- | --- |
| `DatabaseConnectionString` | server=;uid=;pwd=;database=mythingssaver |
| `SQLiteConnectionString` | Data Source=

### Database
Determines which database should be used</br>
Options:
| Database | Setting name | ConnectionString used |
| --- | --- | --- |
| MySQL | mysql | `DatabaseConnectionString` |
| SQLite | sqlite | `SQLiteConnectionString` |

### Origins
Browser security prevents a web page from making requests to a different domain than the one that served the web page.</br>
 This restriction is called the same-origin policy. The same-origin policy prevents a malicious site from reading sensitive data from another site

### JWT 
* `Key` generated key for creating authentication tokens</br>
* `Issuer` Put your domain here. Identifies principal that issued the JWT ([Json Web Token](https://jwt.io/))</br>
* `Audience` Put your domain here. Claim that identifies the recipients that the [JWT](https://jwt.io/) is intended for
</br>

# `👨‍🏫 Q&A`
### Where's my save file? (SQLite)
> You can find your save file inside `save` directory in root folder. It will be called `save.sqlite`

### How do I keep my `appsettings.json` in the same state every debug?
> Move `appsettings.example.json` from examples to `/src/App`.</br>
> It will be automatically moved to your debug folder every build

### Is there any contact to you?
> You can message me on discord `HueByte#0001` or send email to <MyThingsSaver@gmail.com>

### Is App still being developed? 
> Yes. I'm working on it in my free time :)

### What are the future plans?
> Ambitions are big, there's not a lot of time but for future features I want to add more types of "things" you can save, for example: photos, files, links and others. I also want to add plugin support so users can personalize their things saver</br>
