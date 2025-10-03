from flask import Flask, render_template, send_from_directory
from flask_login import LoginManager
from flask_cors import CORS
import os
from dotenv import load_dotenv

load_dotenv()

app = Flask(__name__)

app.config['SECRET_KEY'] = os.environ.get('SECRET_KEY', 'dev-secret-key-change-in-production')
app.config['SQLALCHEMY_DATABASE_URI'] = os.environ.get('DATABASE_URL')
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False

from models import db, User
from auth_routes import auth_bp
from game_routes import game_bp

db.init_app(app)

login_manager = LoginManager()
login_manager.init_app(app)
login_manager.login_view = 'auth.login'

@login_manager.user_loader
def load_user(user_id):
    return User.query.get(int(user_id))

allowed_origins = []
replit_domains = os.environ.get('REPLIT_DOMAINS')
if replit_domains:
    for domain in replit_domains.split(','):
        allowed_origins.extend([f'https://{domain.strip()}', f'http://{domain.strip()}'])
else:
    allowed_origins = ['http://localhost:5000', 'http://127.0.0.1:5000']

CORS(app, 
     resources={r"/api/*": {"origins": allowed_origins}},
     supports_credentials=True)

app.register_blueprint(auth_bp)
app.register_blueprint(game_bp)

with app.app_context():
    db.create_all()

WEBGL_DIR = os.path.join(os.path.dirname(__file__), 'webgl')

@app.after_request
def add_header(response):
    response.headers['Cache-Control'] = 'no-store, no-cache, must-revalidate, post-check=0, pre-check=0, max-age=0'
    response.headers['Pragma'] = 'no-cache'
    response.headers['Expires'] = '-1'
    return response

@app.route('/')
def index():
    webgl_index = os.path.join(WEBGL_DIR, 'index.html')
    if os.path.exists(webgl_index):
        return send_from_directory(WEBGL_DIR, 'index.html')
    return render_template('no_build.html'), 404

@app.route('/docs')
def docs():
    return render_template('index.html')

@app.route('/api-docs')
def api_docs():
    return render_template('api_docs.html')

@app.route('/Build/<path:filename>')
def serve_build(filename):
    return send_from_directory(os.path.join(WEBGL_DIR, 'Build'), filename)

@app.route('/<path:filename>')
def serve_webgl(filename):
    filepath = os.path.join(WEBGL_DIR, filename)
    if os.path.exists(filepath):
        return send_from_directory(WEBGL_DIR, filename)
    return "File not found", 404

@app.route('/health')
def health():
    return {'status': 'healthy', 'database': 'connected'}, 200

if __name__ == '__main__':
    port = int(os.environ.get('PORT', 5000))
    app.run(host='0.0.0.0', port=port, debug=True)
