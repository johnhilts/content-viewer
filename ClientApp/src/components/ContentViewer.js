import React, { Component } from 'react';

const renderButton = (content, onClick) => {
    return (
        <button 
            type='button'
            className='link-button' 
            onClick={onClick}>
            {content}
        </button>
    )
}

const renderFolders = (content, contentName, onContentClick) => {
    return (
        <tr key={content.name}>
          <td>
            {renderButton(content.name, onContentClick.bind(null, contentName, content.contentType))}
          </td>
        </tr>
    )
}

const renderFiles = (content, contentName, onContentClick, currentFolder, showThumbnails) => {
    const contentClass = showThumbnails ? 'thumbnail' : ''
    const isImage = !contentName.endsWith('.mov')
    const image = <img src={`content/${currentFolder}/${contentName}`} alt={contentName} className={contentClass} />
    const video = <video src={`content/${currentFolder}/${contentName}`} title={contentName} className={contentClass} controls />
    const renderElement = isImage ? image : video

    return (
        <tr key={content.name}>
          <td>
            {renderButton(renderElement, onContentClick.bind(null, contentName, content.contentType))}
          </td>
        </tr>
    )
}

const renderContentTable = (contentInfo, onContentClick, currentFolder, currentFileIndex, showThumbnails) => {
      const getContent = (contentInfo) => {
          const isFolder = contentInfo[0].contentType === 0
          const headerTitle = isFolder
          ? 'Folders'
          : 'Files'
          const viewContentInfo = currentFileIndex === -1 
          ? contentInfo
          : contentInfo.filter((f,i) => i === currentFileIndex)
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                  <tr>
                    <th>{headerTitle}</th>
                  </tr>
                </thead>
                <tbody>
                  {viewContentInfo.map(content => {
                          const contentName = content.name === 'Go Back ...'
                          ? '..'
                          : content.name
                      return isFolder
                      ? renderFolders(content, contentName, onContentClick)
                      : renderFiles(content, contentName, onContentClick, currentFolder, showThumbnails)
                      }
                  )}
                </tbody>
            </table>
        )
      }

    const contents = contentInfo.length
      ?
      getContent(contentInfo)
      :
      <div>&nbsp;</div>

    return contents
  }

    const addReturnFolderData = (folderData) => {
        const folders = folderData ? folderData : []
        return [{name: 'Go Back ...', contentType: 0}, ...folders]
    }

export default function ContentViewer(props) {
    let folderData = addReturnFolderData(props.folders)
    let folders = props.loading
      ? <p><em>Loading...</em></p>
      : renderContentTable(folderData, props.onContentClick, props.currentFolder, props.currentFileIndex, props.showThumbnails);

    let files = props.loading
      ? <p><em>Loading...</em></p>
      : renderContentTable(props.files, props.onContentClick, props.currentFolder, props.currentFileIndex, props.showThumbnails);

    return (
      <div>
        <h1 id="tabelLabel" >Content Viewer</h1>
        <p>Select an item to view its content:</p>
        {folders}
        {files}
      </div>
    );
  }
