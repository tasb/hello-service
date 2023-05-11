# Hello microservice

This microservice is first iteration for Hello microservice. It is written in dotnet 7

## How to run

Since it's a containerized application, you need to have docker installed on your machine. Then you can run the following command to build and run the application.

```bash
docker-compose up --build
```

To test the application, you can use the following command

```bash
curl http://localhost:5257/v1
```

To check if service is healthy, you can use the following command

```bash
curl http://localhost:5257/healthcheck
```

If the service is healthy you should get a HTTP 200 response with `healthy` as the response body.

## How to deploy

You can use GitHub Action to deploy this microservice to AWS Fargate. You should have your infrastructure ready in AWS Fargate.

First you need to configure GitHub OIDC integrated with your AWS account. More [here](https://www.cloudtechsimplified.com/github-openid-connect-oidc-aws-secrets/).

Then you only need to update your `main` branch and your container will be deployed.

The container registry used is a public GitHub Packages connected with this repo.

## Reverse Proxy

Since I didn't implement the reverse proxy due to lack of time, I would like to explain what I think that can be a solution.

### Nginx

Using nginx on same container as microservice and making the configuration to redirect the traffic to the microservice.

Configuration will be something like this.

```nginx
server {
    listen        8080;
    location /api {
        proxy_pass         http://127.0.0.1:5257;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
    }
}
```

### YARP

On dotnet environments, we can use [YARP](https://microsoft.github.io/reverse-proxy/) to implement the reverse proxy. It's a library that can be used to implement a reverse proxy on dotnet.

The configuration will be like this:

```json
{
  "ReverseProxy": {
    "Routes": {
      "route1" : {
        "ClusterId": "cluster1",
        "Match": {
          "Path": "/api/{**catch-all}"
        },
      }
    },
    "Clusters": {
      "cluster1": {
        "Destinations": {
          "cluster1/destination1": {
            "Address": "http://127.0.0.1:5257"
          }
        }
      }
    }
  }
}
```

## SSL

To implement SSL on this microservice, we can use [Let's Encrypt](https://letsencrypt.org/) to generate a certificate and use it on the reverse proxy.

## Next Steps

- [ ] Add unit tests
- [ ] Add E2E tests
- [ ] Add Terraform code to create AWS Fargate infrastructure
  