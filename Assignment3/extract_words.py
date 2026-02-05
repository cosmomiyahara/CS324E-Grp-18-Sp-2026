## Zaviyan Tharwani 
## zt3245
# reading novel text
with open("dracula.txt", "r", encoding = "utf-8") as file:
    text = file.read()
print("Characters read:", len(text)) 

# cleaning text
text = text.lower()
clean_text = ""
for char in text:
    if char.isalpha() or char.isspace():
        clean_text += char
words = clean_text.split()
print("Total words:", len(words))

with open("allwords.txt", "w", encoding="utf-8") as out_file:
    for w in words:
        out_file.write(w + "\n")
print("Wrote allwords.txt with", len(words), "words")

word_counts = {}
for w in words:
    if w in word_counts:
        word_counts[w] += 1
    else:
        word_counts[w] = 1
print("Unique words:", len(word_counts))

with open("uniquewords.txt", "w", encoding = "utf-8") as out_file:
    for word, count in word_counts.items():
        if count == 1:
            out_file.write(word + "\n")
print("Wrote uniquewords.txt")

frequency_counts = {}
for count in word_counts.values():
    if count in frequency_counts:
        frequency_counts[count] += 1
    else:
        frequency_counts[count] = 1
with open("wordfrequency.txt", "w", encoding = "utf-8") as out_file:
    for freq in sorted(frequency_counts):
        out_file.write(f"{freq}: {frequency_counts[freq]}\n")
print("Wrote wordfrequency.txt")
