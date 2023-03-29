# SignalR Demo Project

This project demonstrates how to use SignalR for real-time communication between a server and client. The backend is built using .NET 7 with MongoDB data layer, while the frontend is created using React and TypeScript. 

## Technologies Used

- .NET 7
- React
- TypeScript
- MongoDB

## Libraries Used (for prototyping purposes)

- NodeReact
- Betalgo.OpenAI.GPT3

## Prerequisites

To get started with this project, you will need the following installed on your machine:

1. .NET 7 SDK
2. Node.js

This project also uses .NET user secrets. You will need to create a `secrets.json` file with the following format:

```json
{
  "OpenAIServiceOptions": {
    "ApiKey": "<insert your api key here>"
  },
  "MongoDb":
  {
    "ConnectionString": "<insert your connection string here>"
  }
}

## Note

 The application stores user session information in a "x-user" cookie.
