//HTTP 모듈 로딩

let http = require("http");

http.createServer(function (request, response)
{

    response.writeHead(200, {'content-type' : 'text/plain'});

    response.end("Hello world");
}).listen(8000);

console.log("Server running");