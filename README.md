Run your own server to test your app with the following command:

docker run -it --rm -p 0.0.0.0:80:80 snowcollege/gameoflife-server

Then open a web browser and go to http://localhost

Your client needs to:

1) 
    post to /register and send 
    {
       "name": "your name"
    } 
    in the body. You will receive back a token that needs to be included in all future messages.

2) 
    Post to /update with an UpdateRequest in the following format: 
    {
      "token": "your assigned token",
      "generationsComputed": 0
    }

    generations computed (where x is the generation submited):
        x == 0:
            We assume you are waiting, when the game starts we will reply with the board and start your individual timer
        0 <= x <= last_generation:
            Your board is in progress, you must send an update at least every 5 seconds (plz dont spam)
        x == last_generation:
            you will be marked as completed, you must also provide the final board. 
            {
                "token": "your token",
                "GenerationsComputed": <last_generation>,
                "ResultBoard": [
                    {
                        "x": 1,
                        "y": 1
                    }
                ]
            }

   You will get a response back with a gameState ("NotStarted"), generationsToCompute (nullable int), seedBoard (list of Coordinates), isError (bool), errorMessage (string) You need to continue to post to /update every 5 seconds.

Once the game has begun the gameState will become "InProgress", seedBoard will have the initial list of live cells, and generationsToCompute will have a value.
