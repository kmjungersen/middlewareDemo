
# .NET Core Middleware Demo

Appveyor Build status: <img src="https://ci.appveyor.com/api/projects/status/github/kmjungersen/middlewareDemo" alt="">

This is an app meant to demonstrate the power and implementation of .NET Core Middleware.  It features the following technologies:

 * ASP.NET Core 2.0 with C# for cross-platform server-side code
 * Angular 2+ Typescript for client-side code
 * Webpack for building and bundling client-side resources
 * Bootstrap for layout and styling

## Architecture Overview

The project is laid out as follows:

```
├── MyMiddleware
│   ├── MyMiddleware.csproj
├── MyMiddleware.Tests
│   ├── MyMiddleware.Tests.csproj
├── TestApp.Tests
│   ├── TestApp.Tests.csproj
├── TestApp.Web
│   ├── Program.cs
│   ├── Startup.cs
│   ├── TestApp.Web.csproj
├── README.md  <==(You are here)
└── middlewareTest.sln
```

`MyMiddleware` is a standalone Middleware Class library that is inject into the Web App in `Startup.cs`.  For the purposes of this demo, the middleware class library is used again in the `LoggerController` to return the logged data to the client, however that was simply for the sake of convenience.

The injection only requires adding 1 line of code to `Startup.cs`:

```java
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    // Inject our middleware component a the very beginning of the request pipeline.  This will
    // get hit first when requests hit the server, and will get hit last as responses are on their
    // way back to the client.
    app.UseMyMiddleware();
}
```
Is most applications that would be enough, however I also registered the logging service Interface as a Singleton so I could use the same instance for the middleware handling AND the `LoggerController` in the web app:

```java
public void ConfigureServices(IServiceCollection services)
{
    // MVC service is registered here
    services.AddMvc();

    // Register our middleware for use as a service.  It's registered as a Singleton because
    // we only want one instance to be created during the lifespan of the application, rather
    // than creating and destroying it as needed.
    services.AddSingleton<IMiddlewareLogger, MiddlewareLogger>();
}
```

Although I haven't attempted shared dependency injection like this in previous versions of .NET, I have to say I was very impressed at how this is handled in .NET Core.


## Installation Prerequisites

For the purposes of this README, it's assumed that the following software is already installed

 * .NET Core 2.1.4
   * [Downloads](https://www.microsoft.com/net/learn/get-started/macos)
   * (CI Install script for Powershell and Bash included in root directory)
 * Node.JS v6.12.3
   * [Downloads](https://nodejs.org/en/download/)
 * NPM v5.6.0

 To check the current installed version of each of these, run:

 ```bash
 $ dotnet --version
 2.1.4
 $ node -v
 6.12.3
 $ npm -v
 5.6.0
 ```

## Running the Web App

 1. Clone Repo
 1. Install NPM Packages
  ```bash
  # Navigate to the web app within the solution
  $ cd <path_to_repo>/TestApp.Web
  $ npm install
  ```
 1. Restore .NET Packages (also happens at runtime)
  ```bash
  # From the web app directory
  $ dotnet restore
  ```
 1. Build the app (also happens at runtime)
  ```bash
  $ dotnet build
  ```
 1. Run the app
  ```bash
  # Optional: To run in development mode:
  $ set ASPNETCORE_ENVIRONMENT=Development
  # And then to actually run:
  $ dotnet run
  #
  # You should see something like this:
  #
  # Hosting environment: Development
  # Content root path: /<path_to_repo>/TestApp.Web
  # Now listening on: http://localhost:5000
  # Application started. Press Ctrl+C to shut down.
  ```

Once the app is up and running, using it is pretty simple:
  1. Navigate to "Route Testing"
  1. Click around on the line items on this page to trigger sample POST requests to the server
  1. Navigate to the "Results" page
  1. Here we can view all of the logged requests since the application fired up and some details on each request


## Cross-platform Zen

One of my personal biggest reasons for being excited about .NET Core to begin with is true cross platform support.  In previous versions of .NET Core, the cross platform support was a little shaky and unreliable, but I'm *very* happy to say those issues seem to have been resolved!

I wrote this entire app in Visual Studio Code for Mac, and used the terminal for all build and restore actions.  This "text-editor-style" IDE has come a long way from where it was when I last used it a little over a year ago.  With the Omnisharp plugin for intellisense and pre-compilation errors, it was a lot of fun building a .NET app with a text editor on a Mac, and then cloning and building the app seamlessly on Windows.  What an incredible experience!

Additionally, this was my first real project using .NET Core and Angular 2. There was a LOT to learn, but I thoroughly enjoyed the process!

## Misc. Development Notes

Occasionally I had some issues with webpack bundling properly after making restructuring changes. Running `webpack --config webpack.config.vendor.js` in the web app root usually fixed this though.
