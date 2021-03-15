#/bin/bash

docker build -t freelancerzy .
heroku container:push -a freelancerzy web
heroku container:release -a freelancerzy web
