import random
import json
import torch
from model import NeuralNetwork
from nltk_utils import bag_of_words, tokenize
#########################################
from huggingface_hub import hf_hub_download
from llama_cpp import Llama

model_name_or_path="TheBloke/Llama-2-7B-Chat-GGML"
model_basename = "llama-2-7b-chat.ggmlv3.q2_K.bin"

model_path = hf_hub_download(repo_id=model_name_or_path, filename=model_basename)

# GPU
llaama_llm = None
lcpp_llm = Llama(
    model_path=model_path,
    n_threads=2,
    n_batch=512,
    n_gpu_layers=32
)



#########################################
device = torch.device('cuda' if torch.cuda.is_available() else 'cpu')

with open('intents.json', 'r') as f:
    intents = json.load(f)

FILE = "data.pth"
data = torch.load(FILE)

input_size = data["input_size"]
hidden_size = data["hidden_size"]
output_size = data["output_size"]
all_words = data["all_words"]
tags = data["tags"]
model_state = data["model_state"]

model = NeuralNetwork(input_size, hidden_size, output_size).to(device)
model.load_state_dict(model_state)
model.eval()


# ------------------ chatbot ----------------------

bot_name = "Yoghi"

def get_response(msg):
    sentence = tokenize(msg) 
    x = bag_of_words(sentence, all_words)
    x = x.reshape(1, x.shape[0])
    x = torch.from_numpy(x).to(device)

    output = model(x)
    _, predicted = torch.max(output, dim=1)
    tag = tags[predicted.item()]

    probs = torch.softmax(output, dim=1)
    prob = probs[0][predicted.item()]


    if prob.item() > 0.75:
        for intent in intents["intents"]:
           if tag == intent["tag"]:
               # print(f'{bot_name}: {random.choice(intent["responses"])}')
               return random.choice(intent["responses"])

    else:
        prompt_template = f'''SYSTEM: you are a helpful assistant. 
        USER: {msg}
        ASSISTANT: 
        '''

        response = lcpp_llm(prompt=prompt_template, max_tokens=256, temperature=0.5,
                    top_p=0.95, repeat_penalty=1.2, top_k=150, echo=True)


        print(response)
        print("--------------------------------------")
        print(response["choices"][0]["text"])
        print(f'{bot_name}: Scusami, non ho capito, chiedendo a LLAMA ho avuto questo: {response["choices"][0]["text"]}')
        #result = "Scusami, non ho capito, chiedendo a LLAMA ho avuto questo:" + response["choices"][0]["text"]
        # lines = response.splitlines()
        # out_string = lines[3]
        
       
        print("--------------------------------------")
        split_text = response["choices"][0]["text"].split("ASSISTANT:")
        print(split_text)
        print("--------------------------------------")
        print(split_text[1])
        print("--------------------------------------")
        
        result = "Scusami, non ho capito, chiedendo a LLAMA ho avuto questo:" + split_text[1]
        return result
        #return "Scusami, non ho capito."




if __name__ == "__main__":
    print("Ciao! premi Q per uscire")
    while True:
        sentence = input('Tu: ')
        if (sentence == "Q" or sentence == "q") :
            break

        resp = get_response(sentence)
        print(resp)










