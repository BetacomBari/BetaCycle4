from flask import Flask, render_template, request, jsonify
from chat import get_response
from flask_cors import CORS
from recommenderSystem import *


app = Flask(__name__, template_folder='template') #  template_folder='templates'
CORS(app)

@app.route("/chat")
#@app.get("/")
def index_get():
    return render_template("index.html")

@app.post("/predict")
def predict():
    text = request.get_json().get("message")
    if len(text) > 100:
        message = {"answer": "I'm sorry, your query has too many characters for me to process. If you would like to speak to a live agent, say 'I would like to speak to a live agent'"}
        return jsonify(message)
    response = get_response(text)
    message = {"answer": response}
    return jsonify(message)

@app.route("/")
def index():
  # Load data on first request
  if not hasattr(app, 'df_final'):
    app.df_final = read_data()

  # Get product ID from form if submitted, otherwise use default
  product_id = request.args.get("product_id", "841")
  X, customer_mapper, product_mapper, customer_mapper_inv, product_mapper_inv = create_X(app.df_final)
  similar_product = find_similar_item(product_id, X, product_mapper, product_mapper_inv, k=10)

  suggested_product = dict(zip(app.df_final["ProductId"], app.df_final["Name"]))
  return render_template("index_rs.html", product_id=product_id, suggested_product=suggested_product, similar_product=similar_product)



@app.route("/recommendations/<string:item_id>")
def get_recommendations(item_id):
  # Load data on first request
  if not hasattr(app, 'df_final'):
    app.df_final = read_data()

  X, customer_mapper, product_mapper, customer_mapper_inv, product_mapper_inv = create_X(app.df_final)
  similar_product = find_similar_item(item_id, X, product_mapper, product_mapper_inv, k=10)

  suggested_product = dict(zip(app.df_final["ProductId"], app.df_final["Name"]))
  list_to_show_second = []
  for i in similar_product:
      print(f"---{suggested_product[i]}")
      list_to_show_second.append(suggested_product[i])
  return list_to_show_second

  return (list_to_show)


if __name__ == "__main__":
    app.run(debug=True) # host='127.0.0.2'


