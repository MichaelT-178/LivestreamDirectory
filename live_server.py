from livereload import Server

server = Server()

server.watch('*.html')
server.watch('css/*.css')
server.watch('js/*.js')

server.serve(host='localhost', port=8000, root='.')
