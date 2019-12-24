import React, { Component } from 'react';
import ContentViewer from '../components/ContentViewer';

export class ContentViewerContainer extends Component {

  constructor(props) {
    super(props);
    this.state = { currentFolder: '', folders: [], files: [], loading: true, };
  }

  componentDidMount() {
    this.populateContentData();
  }

  render() {
    return (
      <ContentViewer
        loading={this.state.loading}
        folders={this.state.folders}
        files={this.state.files}
        />
    )
  }

  async populateContentData() {
    const response = await fetch('api/content');
    const data = await response.json();
    this.setState({ currentFolder: data.currentFolder, folders: data.folders, files: data.files, loading: false });
  }
}
