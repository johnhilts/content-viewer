import React, { Component } from 'react';
import ContentViewer from '../components/ContentViewer';

export class ContentViewerContainer extends Component {

  constructor(props) {
    super(props);
      this.handleContentClick = this.handleContentClick.bind(this);
      this.parseParentFolder = this.parseParentFolder.bind(this);
      this.handleRemoveClick = this.handleRemoveClick.bind(this);
    this.state = { currentFolder: '', folders: [], files: [], currentFileIndex: -1, loading: true, };
  }

  componentDidMount() {
    this.populateContentData();
  }

    parseParentFolder(folderName) {
        let folders = folderName.split('/').filter(f => f.length)
        if (!folders) {
            return '';
        }
        return folders.filter((f,i) => i < folders.length).reduce((a, f) => a + '/' + f)
    }

    handleRemoveClick(contentName, event) {
        this.removeContentData(contentName)
    }

    handleContentClick(contentName, contentType, currentFileIndex, event) {
        const handleFolderClick = (contentName) => {
            this.setState({loading: true, currentFileIndex: -1, })
            // TODO use an extension (js has extensions??)
            const getParentFolder = contentName === '...'
            if (getParentFolder) {
                this.setState({currentFolder: this.parseParentFolder(contentName)})
            }
            this.rePopulateContentData(contentName, contentType)
        }

        const keepIndexInRange = (index, max) => 
            index < 0 
            ? max
            : index > max 
                ? 0
                : index

        const handleFileClick = (contentName, currentFileIndex) => {
            const fileIndex = currentFileIndex === this.state.currentFileIndex 
                ? this.state.files.findIndex(fi => fi.name === contentName)
                : keepIndexInRange(currentFileIndex, this.state.files.length-1)
            this.setState({ currentFileIndex: fileIndex, showThumbnails: false, });
        }

        event.preventDefault();

        if (contentType === 0) {
            handleFolderClick(contentName)
        }
        else {
            handleFileClick(contentName, currentFileIndex)
        }
    }

  render() {
    return (
      <ContentViewer
        onContentClick={this.handleContentClick}
        onRemoveClick={this.handleRemoveClick}
        loading={this.state.loading}
        currentFolder={this.state.currentFolder}
        folders={this.state.folders}
        files={this.state.files}
        currentFileIndex={this.state.currentFileIndex}
        showThumbnails={this.state.showThumbnails}
        />
    )
  }

  async populateContentData() {
    const response = await fetch('api/content');
    const data = await response.json();
    this.setState({ currentFolder: data.currentFolder, folders: data.folders, files: data.files, showThumbnails: true, loading: false });
  }

    async rePopulateContentData(contentName, contentType) {
        const requestData = {CurrentFolderName: this.state.currentFolder, RequestFolderName: contentName, }
        const response = await fetch('api/content/folder', {method: 'PUT', headers: {'Content-Type': 'application/json'}, body: JSON.stringify(requestData)});
        const data = await response.json();
        this.setState({ currentFolder: data.currentFolder, folders: data.folders, files: data.files, showThumbnails: true, loading: false });
    }

    async removeContentData(contentName) {
        const requestData = {CurrentFolderName: this.state.currentFolder, RequestFileName: contentName, }
        const response = await fetch('api/content/folder', {method: 'DELETE', headers: {'Content-Type': 'application/json'}, body: JSON.stringify(requestData)});
        const data = await response.json();
        this.setState({ files: data.files, });
    }
}
