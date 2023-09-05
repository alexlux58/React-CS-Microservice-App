# React-CS-Microservice-App

## Overview

This project is a microservices-based application built with React and C#. It aims to demonstrate how to create, manage, and deploy a scalable and maintainable microservices architecture.

## Features

- **Auction Service**: Manages auctions, items, and their statuses.
- **React Frontend**: A user-friendly interface built with React.
- **Docker Support**: Containerized services for easy deployment.

## Prerequisites

- .NET SDK
- Node.js
- Docker
- Visual Studio Code or any IDE of your choice

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/alexlux58/React-CS-Microservice-App.git
```

### Install Dependencies

Navigate to the src/AuctionService directory and run:
dotnet restore

For the React frontend, navigate to its directory and run:
npm install

### Run the Application

Use Docker Compose to start all services:
docker-compose up
