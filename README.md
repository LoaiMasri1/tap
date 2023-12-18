# Travel and Accommodation Booking Platform

This project implements a comprehensive hotel booking system with a discount management feature using ASP.NET Core API. It allows users to browse hotels, view room details, make reservations, and apply discount codes to reduce their booking costs. Hotel owners can manage hotel information, room types, and discounts through API endpoints.

## Prerequisites

- ASP.NET Core 7 SDK installed
- Docker installed

## Setup Guide

1. Clone the Project: Clone the project repository to your local machine using Git.

```bash
git clone github.com/LoaiMasri1/tap.git
```

2. Run the Project: Navigate to the project directory and run the following commands to start the project.

```bash
docker-compose -f docker-compose.yml -f docker-compose.override.yml build
```

```bash
docker-compose -f docker-compose.yml -f docker-compose.override.yml up
```

3. Access the Project: Open your browser and navigate to `http://localhost:7194` to access the project.

4. Stop the Project: To stop the project, run the following command.

```bash
docker-compose -f docker-compose.yml -f docker-compose.override.yml down
```

## API Documentation

The API documentation is available at `http://localhost:7194/swagger/index.html`.
