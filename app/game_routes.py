from flask import Blueprint, request, jsonify
from flask_login import login_required, current_user
from models import db, GameSave

game_bp = Blueprint('game', __name__, url_prefix='/api/game')

@game_bp.route('/save', methods=['POST'])
@login_required
def save_game():
    data = request.get_json()
    
    if not data:
        return jsonify({'error': 'No data provided'}), 400
    
    save_data = data.get('save_data', {})
    character_name = data.get('character_name', '')
    resources = data.get('resources', {})
    decisions_count = data.get('decisions_count', 0)
    
    game_save = GameSave(
        user_id=current_user.id,
        character_name=character_name,
        save_data=save_data,
        resources=resources,
        decisions_count=decisions_count
    )
    
    db.session.add(game_save)
    db.session.commit()
    
    return jsonify({
        'message': 'Game saved successfully',
        'save': game_save.to_dict()
    }), 201

@game_bp.route('/saves', methods=['GET'])
@login_required
def get_saves():
    saves = GameSave.query.filter_by(user_id=current_user.id).order_by(GameSave.updated_at.desc()).all()
    
    return jsonify({
        'saves': [save.to_dict() for save in saves]
    }), 200

@game_bp.route('/save/<int:save_id>', methods=['GET'])
@login_required
def get_save(save_id):
    save = GameSave.query.filter_by(id=save_id, user_id=current_user.id).first()
    
    if not save:
        return jsonify({'error': 'Save not found'}), 404
    
    return jsonify({'save': save.to_dict()}), 200

@game_bp.route('/save/<int:save_id>', methods=['PUT'])
@login_required
def update_save(save_id):
    save = GameSave.query.filter_by(id=save_id, user_id=current_user.id).first()
    
    if not save:
        return jsonify({'error': 'Save not found'}), 404
    
    data = request.get_json()
    
    if 'save_data' in data:
        save.save_data = data['save_data']
    if 'character_name' in data:
        save.character_name = data['character_name']
    if 'resources' in data:
        save.resources = data['resources']
    if 'decisions_count' in data:
        save.decisions_count = data['decisions_count']
    
    db.session.commit()
    
    return jsonify({
        'message': 'Save updated successfully',
        'save': save.to_dict()
    }), 200

@game_bp.route('/save/<int:save_id>', methods=['DELETE'])
@login_required
def delete_save(save_id):
    save = GameSave.query.filter_by(id=save_id, user_id=current_user.id).first()
    
    if not save:
        return jsonify({'error': 'Save not found'}), 404
    
    db.session.delete(save)
    db.session.commit()
    
    return jsonify({'message': 'Save deleted successfully'}), 200

@game_bp.route('/stats', methods=['GET'])
@login_required
def get_stats():
    saves = GameSave.query.filter_by(user_id=current_user.id).all()
    
    total_saves = len(saves)
    total_decisions = sum(save.decisions_count for save in saves)
    characters_played = list(set(save.character_name for save in saves if save.character_name))
    
    return jsonify({
        'total_saves': total_saves,
        'total_decisions': total_decisions,
        'characters_played': characters_played,
        'latest_save': saves[0].to_dict() if saves else None
    }), 200
