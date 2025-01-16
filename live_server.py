import subprocess
from livereload import Server

def write_to_clipboard():
    output = 'http://localhost:8000'
    process = subprocess.Popen('pbcopy', env={'LANG': 'en_US.UTF-8'}, stdin=subprocess.PIPE)
    process.communicate(output.encode('utf-8'))
    print("\nhttp://localhost:8000 was copied to clipboard!\n")

write_to_clipboard()

server = Server()

server.watch('*.html')
server.watch('css/*.css')
server.watch('js/*.js')

server.serve(host='localhost', port=8000, root='.')
