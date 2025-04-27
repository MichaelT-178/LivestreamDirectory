import json

# Load the instruments
with open("instruments.json", 'r', encoding='utf-8') as file:
    content = json.load(file)
    instruments = content['Instruments']

# Fix the IDs
for idx, instrument in enumerate(instruments, start=1):
    instrument['id'] = idx

# Write the updated instruments back to the file
with open("instruments.json", 'w', encoding='utf-8') as file:
    json.dump({"Instruments": instruments}, file, ensure_ascii=False, indent=4)

print("IDs updated successfully!")
