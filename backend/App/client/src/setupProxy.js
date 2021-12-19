const createProxyMiddleware = require("http-proxy-middleware");
const { env } = require("process");

// const target = env.ASPNETCORE_HTTPS_PORT
//   ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
//   : env.ASPNETCORE_URLS
//   ? env.ASPNETCORE_URLS.split(";")[0]
//   : "http://localhost:3000";

const target = "https://localhost:5001";

module.exports = function (app) {
  const appProxy = createProxyMiddleware({
    target: target,
    secure: false,
  });

  app.use("/api", appProxy);
};
