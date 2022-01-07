const { createProxyMiddleware } = require("http-proxy-middleware");

module.exports = function (app) {
  const appProxy = createProxyMiddleware("/api", {
    target: "https://localhost:5001/",
    secure: false,
  });

  app.use(appProxy);
};
