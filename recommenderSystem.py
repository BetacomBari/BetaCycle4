import pandas as pd
from scipy.sparse import csr_matrix
import numpy as np
from sklearn.neighbors import NearestNeighbors
from flask import Flask, render_template, request, jsonify
from flask_cors import CORS

df_final = pd.read_csv("expanded_data.csv")

def read_data():
  df_final = pd.read_csv("expanded_data.csv")
  return df_final
# ratings = pd.DataFrame(df_final["customerId", "ProductId", 'rating'])
# create the matrix with users and ratings
def create_X(df_final):
    M = df_final["customerId"].nunique()
    N = df_final["ProductId"].nunique()

    # Creates dictionaries (user_mapper, movie_mapper) to map original user/movie IDs to their index in the matrix (0
    # to M-1, 0 to N-1).
    customer_mapper = dict(zip(np.unique(df_final["customerId"]), list(range(M))))
    product_mapper = dict(zip(np.unique(df_final["ProductId"]), list(range(N))))

    # Creates dictionaries (user_inv_mapper, movie_inv_mapper) to map back from the matrix index to the original
    # user/movie ID.
    customer_mapper_inv = dict(zip(list(range(M)), np.unique(df_final["customerId"])))
    product_mapper_inv = dict(zip(list(range(N)), np.unique(df_final["ProductId"])))

    customer_index = [customer_mapper[i] for i in df_final["customerId"]]
    product_index = [product_mapper[i] for i in df_final["ProductId"]]

    X = csr_matrix((df_final["rating"], (customer_index, product_index)), shape=(M, N))

    return X, customer_mapper, product_mapper, customer_mapper_inv, product_mapper_inv


X, customer_mapper, product_mapper, customer_mapper_inv, product_mapper_inv = create_X(df_final)


# now we find the similar item (with cosine similarity)
def find_similar_item(product_id, X, product_mapper, product_mapper_inv, k, metric="cosine"):
    X = X.T
    neighbour_ids = []

    product_ind = product_mapper[product_id]
    product_vect = X[product_ind]
    if isinstance(product_vect, np.ndarray):
        product_vect = product_vect.reshape(1, -1)
    # we use k+1 because kNN output includes the movieId of interest, kNN = k Nearest Neighours
    kNN = NearestNeighbors(n_neighbors=k + 1, algorithm="brute", metric=metric)
    kNN.fit(X)
    neighbour = kNN.kneighbors(product_vect, return_distance=False)
    # now let's populate the neighbours
    for i in range(0, k):
        n = neighbour.item(i)
        neighbour_ids.append(product_mapper_inv[n])
    # remove the first because it is the element we want to find its neighbours
    neighbour_ids.pop(0)
    return neighbour_ids


similar_product = find_similar_item("841", X, product_mapper, product_mapper_inv, k=10)
print(similar_product)

suggested_product = dict(zip(df_final["ProductId"], df_final["Name"]))
product_id = "901"

similar_product = find_similar_item(product_id, X, product_mapper, product_mapper_inv, k=10)
#movie_title = suggested_product[product_id]


print(f"Because you have bought {suggested_product[product_id]}, we suggest you to buy: ")
for i in similar_product:
  print(f"---{suggested_product[i]}")


print("__________________________________________")
print("__________________________________________")
print("__________________________________________")
print("__________________________________________")
def runRecommenderSystem():
    X, customer_mapper, product_mapper, customer_mapper_inv, product_mapper_inv = create_X(df_final)
    similar_product = find_similar_item("841", X, product_mapper, product_mapper_inv, k=10)
    print(f"Because you have bought {suggested_product[product_id]}, we suggest you to buy: ")
    for i in similar_product:
        print(f"---{suggested_product[i]}")
    return similar_product


app = Flask(__name__, template_folder='template')
CORS(app)

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



@app.route("/api/recommendations/<string:item_id>")
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
  app.run(debug=True)