#/bin/bash

if [ -z $1]
then
    echo "Wprowadź nazwę projektu"
else
    docker build -t $1 .
    heroku container:push -a $1 web
    heroku container:release -a $1 web
fi
