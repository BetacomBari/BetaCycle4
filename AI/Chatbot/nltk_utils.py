from nltk.stem.porter import PorterStemmer
import nltk
import numpy as np
nltk.download('punkt')


stemmer = PorterStemmer()


def tokenize(sentence):
    return nltk.word_tokenize(sentence)


def stem(word):
    return stemmer.stem(word.lower())


def bag_of_words(tokenized_sentence, all_words):
    '''
    THIS IS AN EXAMPLE

    sentence = ["ciao", "come", "stai"]
    words = ["buongiorno", "buonasera", "ciao", "arrivederci", "come", "stai", "grazie"]
    bag = [      0             0           1        0             1       1       0    ]
    '''

    tokenized_sentence = [stem(w) for w in tokenized_sentence]

    # first, we set all the number of the vector to zero,
    # then we change our hot words to 1
    bag = np.zeros(len(all_words), dtype=np.float32)
    for idx, w in enumerate(all_words):
        if w in tokenized_sentence:
            bag[idx] = 1.0
    return bag


'''
sentence = ["ciao", "come", "stai"]
words = ["buongiorno", "buonasera", "ciao",
         "arrivederci", "come", "stai", "grazie"]
bag = bag_of_words(sentence, words)
print(bag)

if you want to try remove this block of comment
'''

