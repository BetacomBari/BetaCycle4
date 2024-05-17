# conversation = """SYSTEM: you are a helpful assistant. 
# USER: chi è reus
# ASSISTANT:
#  Reus is a professional footballer who plays as an attacking midfielder for Barcelona and the Spain national team. He was born on April 7, 1992 in Tarragona, Catalonia, Spain."""

# # Split the conversation by lines
# lines = conversation.splitlines()

# # Find the line that starts with "ASSISTANT:"
# assistant_line = [line for line in lines if line.startswith("ASSISTANT:")]

# try_string = lines[3]
# # Extract the assistant text (assuming there's only one line)
# if assistant_line:
#   assistant_text = assistant_line[0].split(":")[1].strip()
# else:
#   assistant_text = "none"

# # Print the assistant text
# print(try_string)
# print("-----------------")
# print(assistant_text)



string_test = 'SYSTEM: you are a helpful assistant. \n        USER: chi è tesla\n        ASSISTANT: provaaaaaaa        '
split_text = string_test.split("ASSISTANT:")
print(split_text[1])  
