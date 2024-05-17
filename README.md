# Chatbot

This projeect is made by **Alberto Panico**, **Angelo Nardella** and **Andrea Nigro**.  
We have created a chatbot for Tunnelbear that is able to give answers to typical questiond for a VPN service. 


## Install
```bash
cd chatbot
python3 -m venv venv
# linux/mac
./venv/bin/activate
# windows
./venv/Script/activate
# 
pip install torch numpy nltk flask

# train
python python/train.py
# execute
python python/chat.py

```

## How Does It Works?

1. ### DEFINE INTETNTS. 
We have defined our intents in intents.json, with tag and pattern, this help us for the training of the model.

2. ### NLTK.
We use the nltk library to work with the words (`nltk_utils.py`), in particular we use nltk to tokenize and stem the text.

3. ### CREATE OUR NEURAL NETWORK.
We use a feed forward neural network. The input layer is fully-connected, it has the number of pattern as dimension, then we have 2 hidden layer, in the end we have an output layer with the number of classes as dimension. Now we apply a softmax to get y (the response is given if it has at least 75% according to the training part).


4. ### TRAINING OF THE MODEL.
Now we want to train the model (`train.py`), so we import the function we had written in `nltk_utils.py`.
We use [pytorch](https://pytorch.org/).

5. ### CHATBOT.
We can find the logic of the chatbot in `app.js`. The chatbot (Yoghi is the name we have assigned to it) initially says to you what he can do with a welcome message, then you can type the message. He is able to answer at typical questions you can ask to a VPN service. If the message is longer then 100 character or you ask something he is not trained for, he reply with an error message.

6. ### CONNECT TO THE WEBSITE.
We use Flask (`app.py`) to create our API.
With @app.get("/") we return our webpage's template and with @app.post("/predict") we predict the response to the question you have asked, according to the logic and the training part.

