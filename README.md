# RabbitMQTest

It is a prototype project with communication between inner projects via RabbitMQ

## Running

- Run `docker-compose build` to build containers
- Run `docker-compose up` to run them

## Acting
When application is running you have an ability to handle messages.
- Navigate to `localhost:7011` to see logs of handling
- Navigate to `localhost:7000/event-1` to create a message from project A to project B
- Navigate to `localhost:7000/event-2` to create another message from project A to project B
- Navigate to `localhost:7001/event-1` to create a message from project B to project A
- Navigate to `localhost:7001/event-2` to create another message from project B to project A
