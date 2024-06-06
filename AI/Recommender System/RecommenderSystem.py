import pandas as pd
from scipy.sparse import csr_matrix
import numpy as np
from sklearn.neighbors import NearestNeighbors




# ratings = pd.DataFrame(df_final["customerId", "ProductId", 'rating'])
# create the matrix with users and ratings
def create_X():
    df_final = pd.read_csv("expanded_data.csv")
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
movie_title = suggested_product[product_id]


print(f"Because you have bought {suggested_product[product_id]}, we suggest you to buy: ")
for i in similar_product:
  print(f"---{suggested_product[i]}")