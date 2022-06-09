// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks
{
    public sealed class ZipFileCreateFromDirectory : Task
    {
        /// <summary>
        /// The path to the directory to be archived.
        /// </summary>
        [Required]
        public string SourceDirectory { get; set; }

        /// <summary>
        /// The path of the archive to be created.
        /// </summary>
        [Required]
        public string DestinationArchive { get; set; }

        /// <summary>
        /// Indicates if the destination archive should be overwritten if it already exists.
        /// </summary>
        public bool OverwriteDestination { get; set; }

        /// <summary>
        /// If zipping an entire folder without exclusion patterns, whether to include the folder in the archive.
        /// </summary>
        public bool IncludeBaseDirectory { get; set; }

        /// <summary>
        /// An item group of regular expressions for content to exclude from the archive.
        /// </summary>
        public ITaskItem[] ExcludePatterns { get; set; }

        public override bool Execute()
        {
            try
            {
                if (File.Exists(DestinationArchive))
                {
                    if (OverwriteDestination == true)
                    {
                        Log.LogMessage(MessageImportance.Low, "{0} already existed, deleting before zipping...", DestinationArchive);
                        File.Delete(DestinationArchive);
                    }
                    else
                    {
                        Log.LogWarning("'{0}' already exists. Did you forget to set '{1}' to true?", DestinationArchive, nameof(OverwriteDestination));
                    }
                }

                Log.LogMessage(MessageImportance.High, "Compressing {0} into {1}...", SourceDirectory, DestinationArchive);
                if (!Directory.Exists(Path.GetDirectoryName(DestinationArchive)))
                    Directory.CreateDirectory(Path.GetDirectoryName(DestinationArchive));

                if (ExcludePatterns == null)
                {
                    ZipFile.CreateFromDirectory(SourceDirectory, DestinationArchive, CompressionLevel.Optimal, IncludeBaseDirectory);
                }
                else
                {
                    // convert to regular expressions
                    Regex[] regexes = new Regex[ExcludePatterns.Length];
                    for (int i = 0; i < ExcludePatterns.Length; ++i)
                        regexes[i] = new Regex(ExcludePatterns[i].ItemSpec, RegexOptions.IgnoreCase);

                    using (FileStream writer = new FileStream(DestinationArchive, FileMode.CreateNew))
                    {
                        using (ZipArchive zipFile = new ZipArchive(writer, ZipArchiveMode.Create))
                        {
                            var files = Directory.GetFiles(SourceDirectory, "*", SearchOption.AllDirectories);

                            foreach (var file in files)
                            {
                                // look for a match
                                bool foundMatch = false;
                                foreach (var regex in regexes)
                                {
                                    if (regex.IsMatch(file))
                                    {
                                        foundMatch = true;
                                        break;
                                    }
                                }

                                if (foundMatch)
                                {
                                    Log.LogMessage(MessageImportance.Low, "Excluding {0} from archive.", file);
                                    co#include "BaseVSShader.h"

// Note: you have to run buildshaders.bat to generate these files from the FXC code.
#include "sdk_lightmap_ps20.inc"
#include "sdk_lightmap_vs20.inc"

BEGIN_VS_SHADER( SDK_Lightmap, "Help for SDK_Lightmap" )

	BEGIN_SHADER_PARAMS
		SHADER_PARAM( BUMPMAP, SHADER_PARAM_TYPE_TEXTURE, "shadertest/BaseTexture", "base texture" )
		SHADER_PARAM( BUMPFRAME, SHADER_PARAM_TYPE_INTEGER, "0", "frame number for $bumpmap" )
	END_SHADER_PARAMS

	// Set up anything that is necessary to make decisions in SHADER_FALLBACK.
	SHADER_INIT_PARAMS()
	{
		if( !params[BUMPFRAME]->IsDefined() )
		{
			params[BUMPFRAME]->SetIntValue( 0 );
		}
	}

	SHADER_FALLBACK
	{
		return 0;
	}

	// Note: You can create member functions inside the class definition.
	int SomeMemberFunction()
	{
		return 0;
	}

	SHADER_INIT
	{
		LoadTexture( BASETEXTURE );
	}

	SHADER_DRAW
	{
		SHADOW_STATE
		{
			// Enable the texture for base texture and lightmap.
			pShaderShadow->EnableTexture( SHADER_TEXTURE_STAGE0, true );
			pShaderShadow->EnableTexture( SHADER_TEXTURE_STAGE1, true );

			sdk_lightmap_vs20_Static_Index vshIndex;
			pShaderShadow->SetVertexShader( "sdk_lightmap_vs20", vshIndex.GetIndex() );

			sdk_lightmap_ps20_Static_Index pshIndex;
			pShaderShadow->SetPixelShader( "sdk_lightmap_ps20", pshIndex.GetIndex() );

			DefaultFog();
		}
		DYNAMIC_STATE
		{
			BindTexture( SHADER_TEXTURE_STAGE0, BASETEXTURE, FRAME );
			pShaderAPI->BindLightmap( SHADER_TEXTURE_STAGE1 );
		}
		Draw();
	}
END_SHADER
ntinue;BEGIN_VS_SHADER( [shader name], [help string] ) / END_SHADER
SHADER_PARAM( [param name], [param type], [default value], [help string] )
SHADER_PARAM( LIGHT_COLOR, SHADER_PARAM_TYPE_VEC3, "1 0 0", "This is the directional light color." )

            XrCompositionLayerQuad
                                }

                                var relativePath = MakeRelativePath(SourceDirectory, file);
            SHADER_PARAM( LIGHT_COLOR, SHADER_PARAM_TYPE_VEC3, "1 0 0", "This is the directional light color." )
                                zipFile.CreateEntryFromFile(file, relativePath, CompressionLevel.Optimal);
            SHADER_INIT_PARAMS
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // We have 2 log calls because we want a nice error message but we also want to capture the callstack in the log.
                Log.LogError("An exception has occurred while trying to compress '{0}' into '{1}'.", SourceDirectory, DestinationArchive);
                Log.LogMessage(MessageImportance.Low, e.ToString());
                return false;
            }

            return true;
        }

        private string MakeRelativePath(string root, string subdirectory)
        {
            if (!subdirectory.StartsWith(root))
                throw new Exception(string.Format("'{0}' is not a subdirectory of '{1}'.", subdirectory, root));

            // returned string should not start with a directory separator
            int chop = root.Length;
            if (subdirectory[chop] == Path.DirectorySeparatorChar)
                ++chop;

            return subdirectory.Substring(chop);
        }
    }
}
